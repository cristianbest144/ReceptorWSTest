using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Web;

namespace Shared.Network
{
    public static class NetworkExtensions
    {
        public static string ToQueryString(this object obj, bool addInterrogation = true)
        {
            var content = addInterrogation ? "?" : string.Empty;
            foreach (var property in obj.GetType().GetRuntimeProperties())
            {
                var value = property.GetValue(obj);
                if (value is string valueString)
                    content += $"{property.Name}={HttpUtility.UrlEncode(valueString)}&";
            }
            return content;
        }

        public static string ToCustomString(this object obj, string separator = ",", bool addFieldName = false)
        {
            var content = string.Empty;
            foreach (var property in obj.GetType().GetRuntimeProperties())
            {
                var value = property.GetValue(obj);
                if (value is string valueString)
                {
                    if (addFieldName)
                        content += $"{property.Name}=";
                    content += $"{valueString}{separator}";
                }
            }
            return content;
        }

        public static MultipartFormDataContent ToMultipartFormDataContent(this object obj)
        {
            var content = new MultipartFormDataContent();
            foreach (var property in obj.GetType().GetRuntimeProperties())
            {
                var value = property.GetValue(obj);

                if (value is string valueString)
                    content.Add(new StringContent(valueString), property.Name);
            }
            return content;
        }

        public static FormUrlEncodedContent ToFormUrlEncodedContent(this object obj, bool onlyStrings, bool urlEncode)
        {
            var content = new List<KeyValuePair<string, string>>();
            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (onlyStrings && value is string || !onlyStrings)
                    content.Add(new KeyValuePair<string, string>(property.Name, urlEncode ? HttpUtility.UrlEncode(value.ToString()) : value.ToString()));
            }
            return new FormUrlEncodedContent(content);
        }
    }
}