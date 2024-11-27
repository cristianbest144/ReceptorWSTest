using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Extensions;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Shared.Network
{
    public class HttpServiceClient : IDisposable
    {
        private readonly HttpClientHandler _httpClientHandler;
        private readonly HttpClient _httpClient;

        public enum ERequestContentType
        {
            Json,
            Xml,
            Xml2,
            MultipartForm,
            UrlEncodedStrings,
            UrlEncoded,
            String,
            Custom
        }

        public enum ESerializationJsonFormatting
        {
            Default,
            CamelCase,
            SnakeCase,
        }

        public enum EResponseContentType
        {
            File,
            Html,
            Json,
            Mixed,
            Text,
            Xml,
        }

        public ESerializationJsonFormatting SerializationJsonFormatting { get; set; } =
            ESerializationJsonFormatting.Default;

        public ERequestContentType RequestContentType { get; set; } = ERequestContentType.Json;
        public EResponseContentType ResponseContentType { get; set; } = EResponseContentType.Json;
        public bool UrlEncodeBody { get; set; }

        public void SetTimeout(TimeSpan timeout)
        {
            _httpClient.Timeout = timeout;
        }

        public HttpServiceClient()
        {
            _httpClientHandler = new HttpClientHandler();
            _httpClient = new HttpClient(_httpClientHandler);

            _httpClient.Timeout = new TimeSpan(0, 1, 0);
        }

        public void AddHttpHeader(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void SetHttpHeaderAuthentication(AuthenticationHeaderValue headerValue)
        {
            _httpClient.DefaultRequestHeaders.Authorization = headerValue;
        }

        private static async void ProcessResponseErrorsAsync<T>(HttpResponseMessage response, ServiceResponse<T> sr)
        {
            var errorCode = Errors.HttpErrors.HTTP_ANOTHER_ERROR;
            var errorMessage = response.StatusCode.ToString();

            var errorDetail = await response.Content.ReadAsStringAsync();
            sr.NetworkOperationResponseBody = errorDetail;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    errorCode = Errors.HttpErrors.HTTP_400_BAD_REQUEST;
                    break;
                case HttpStatusCode.Unauthorized:
                    errorCode = Errors.HttpErrors.HTTP_401_UNAUTHORIZED;
                    break;
                case HttpStatusCode.Forbidden:
                    errorCode = Errors.HttpErrors.HTTP_403_FORBIDDEN;
                    break;
                case HttpStatusCode.NotFound:
                    errorCode = Errors.HttpErrors.HTTP_404_NOT_FOUND;
                    break;
                case HttpStatusCode.InternalServerError:
                    errorCode = Errors.HttpErrors.HTTP_500_INTERNAL_SERVER_ERROR;
                    break;
                case HttpStatusCode.BadGateway:
                    errorCode = Errors.HttpErrors.HTTP_502_BAD_GATEWAY;
                    break;
            }

            sr.AddError(errorCode, errorMessage, errorDetail);
        }

        private async Task ProcessResponseFileAsync(ServiceResponse<byte[]> sr,
            HttpResponseMessage response)
        {
            sr.NetworkOperationStatus = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    sr.Data = await response.Content.ReadAsByteArrayAsync();
                    sr.NetworkOperationResponseBody = "<File>";
                }
            }
            else
                ProcessResponseErrorsAsync(response, sr);
        }

        private async Task ProcessResponseAsync<T>(ServiceResponse<T> sr,
            HttpResponseMessage response)
        {
            sr.NetworkOperationStatus = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) return;

                if (ResponseContentType.IsIn(EResponseContentType.Mixed, EResponseContentType.File))
                    sr.NetworkOperationResponseRawBody = await response.Content.ReadAsByteArrayAsync();
                if (ResponseContentType.IsIn(EResponseContentType.Mixed, EResponseContentType.Html,
                    EResponseContentType.Json, EResponseContentType.Xml, EResponseContentType.Text))
                    sr.NetworkOperationResponseBody = await response.Content.ReadAsStringAsync();

                sr.NetworkOperationResponseContentType = response.Content.Headers.ContentType.ToString();

                if (int.TryParse(sr.NetworkOperationResponseBody, out int id))
                    sr.ReturnValue = id;
                else
                {
                    switch (ResponseContentType)
                    {
                        case EResponseContentType.Html:
                        case EResponseContentType.Mixed:
                        case EResponseContentType.Text:
                            sr.Data = (T)Convert.ChangeType(sr.NetworkOperationResponseBody, typeof(T));
                            break;
                        case EResponseContentType.Json:
                            sr.Data = DeserializeJsonObject<T>(sr.NetworkOperationResponseBody);
                            break;
                        case EResponseContentType.File:
                            sr.ReturnStream = await response.Content.ReadAsStreamAsync();
                            break;
                        case EResponseContentType.Xml:
                            var serializer = new XmlSerializer(typeof(T));
                            using (var reader = new StringReader(sr.NetworkOperationResponseBody))
                            {
                                sr.Data = (T)serializer.Deserialize(reader);
                            }
                            break;
                        default:
                            sr.ReturnContent = sr.NetworkOperationResponseBody;
                            break;
                    }
                }
            }
            else
                ProcessResponseErrorsAsync(response, sr);
        }

        private string SerializeJsonObject(object data)
        {
            NamingStrategy namingStrategy = null;

            switch (SerializationJsonFormatting)
            {
                case ESerializationJsonFormatting.CamelCase:
                    namingStrategy = new CamelCaseNamingStrategy();
                    break;
                case ESerializationJsonFormatting.SnakeCase:
                    namingStrategy = new SnakeCaseNamingStrategy();
                    break;
                default:
                    namingStrategy = new DefaultNamingStrategy();
                    break;
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = namingStrategy
            };
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

        private T DeserializeJsonObject<T>(string data)
        {
            NamingStrategy namingStrategy = null;

            switch (SerializationJsonFormatting)
            {
                case ESerializationJsonFormatting.CamelCase:
                    namingStrategy = new CamelCaseNamingStrategy();
                    break;
                case ESerializationJsonFormatting.SnakeCase:
                    namingStrategy = new SnakeCaseNamingStrategy();
                    break;
                default:
                    namingStrategy = new DefaultNamingStrategy();
                    break;
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = namingStrategy
            };
            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

        public async Task<ServiceResponse<object>> GetAsync(bool authorization, string url,
            string queryParameters = null)
        {
            return await GetAsync<object>(authorization, url, queryParameters);
        }

        public async Task<ServiceResponse<T>> GetAsync<T>(bool authorization, string url, string queryParameters = null)
        {
            var sr = new ServiceResponse<T>();

            if (queryParameters != null)
                url = url + "?" + queryParameters;

            sr.NetworkOperationRequestMethod = "GET";
            sr.NetworkOperationRequestUri = url;
            sr.NetworkOperationRequestBody = queryParameters;

            try
            {
                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);
                await ProcessResponseAsync(sr, response);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }

            return sr;
        }

        public async Task<ServiceResponse<object>> PostAsync(bool authorization, string url, object body = null)
        {
            return await PostAsync<object>(authorization, url, body);
        }

        public async Task<ServiceResponse<T>> PostAsync<T>(bool authorization, string url, object body = null)
        {
            var sr = new ServiceResponse<T>();

            try
            {
                if (body == null)
                    body = new { };

                sr.NetworkOperationRequestMethod = "POST";
                sr.NetworkOperationRequestUri = url;
                sr.NetworkOperationRequestBody = RequestContentType == ERequestContentType.String
                    ? body.ToString()
                    : JsonConvert.SerializeObject(body);

                HttpContent bodyContent;
                switch (RequestContentType)
                {
                    case ERequestContentType.Json:
                        bodyContent = new StringContent(SerializeJsonObject(body), Encoding.UTF8, "application/json");
                        break;
                    case ERequestContentType.MultipartForm:
                        bodyContent = body.ToMultipartFormDataContent();
                        break;
                    case ERequestContentType.UrlEncodedStrings:
                        bodyContent = body.ToFormUrlEncodedContent(true, true);
                        break;
                    case ERequestContentType.UrlEncoded:
                        bodyContent = body.ToFormUrlEncodedContent(false, false);
                        break;
                    case ERequestContentType.String:
                        bodyContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
                        break;
                    case ERequestContentType.Xml:
                        bodyContent = new StringContent(body.ToString(), Encoding.UTF8, "application/soap+xml");
                        break;
                    case ERequestContentType.Xml2:
                        bodyContent = new StringContent(body.ToString());
                        break;
                    default:
                        bodyContent = null;
                        break;
                }

                var response = await _httpClient.PostAsync(Uri.EscapeUriString(url), bodyContent);
                await ProcessResponseAsync<T>(sr, response);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }

            return sr;
        }

        public async Task<ServiceResponse<object>> PutAsync(bool authorization, string url, object body = null)
        {
            return await PutAsync<object>(authorization, url, body);
        }

        public async Task<ServiceResponse<T>> PutAsync<T>(bool authorization, string url, object body)
        {
            var sr = new ServiceResponse<T>();

            try
            {
                if (body == null)
                    body = new { };

                sr.NetworkOperationRequestMethod = "PUT";
                sr.NetworkOperationRequestUri = url;
                sr.NetworkOperationRequestBody = JsonConvert.SerializeObject(body);

                HttpContent bodyContent;
                switch (RequestContentType)
                {
                    case ERequestContentType.Json:
                        bodyContent = new StringContent(SerializeJsonObject(body), Encoding.UTF8,
                            "application/json");
                        break;
                    case ERequestContentType.MultipartForm:
                        bodyContent = body.ToMultipartFormDataContent();
                        break;
                    case ERequestContentType.UrlEncodedStrings:
                        bodyContent = body.ToFormUrlEncodedContent(true, UrlEncodeBody);
                        break;
                    case ERequestContentType.UrlEncoded:
                        bodyContent = body.ToFormUrlEncodedContent(false, UrlEncodeBody);
                        break;
                    case ERequestContentType.Custom:
                        bodyContent = (HttpContent)body;
                        break;
                    default:
                        bodyContent = null;
                        break;
                }

                var response = await _httpClient.PutAsync(url, bodyContent);
                await ProcessResponseAsync<T>(sr, response);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }

            return sr;
        }

        public async Task<ServiceResponse<object>> DeleteAsync(bool authorization, string url,
            string queryParameters = null)
        {
            return await DeleteAsync<object>(authorization, url, queryParameters);
        }

        public async Task<ServiceResponse<T>> DeleteAsync<T>(bool authorization, string url,
            string queryParameters = null)
        {
            var sr = new ServiceResponse<T>();

            if (queryParameters != null)
                url = url + "?" + queryParameters;

            sr.NetworkOperationRequestMethod = "DELETE";
            sr.NetworkOperationRequestUri = url;
            sr.NetworkOperationRequestBody = queryParameters;

            try
            {
                var response = await _httpClient.DeleteAsync(url);
                await ProcessResponseAsync<T>(sr, response);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }

            return sr;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _httpClientHandler.Dispose();
        }
    }
}