using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shared
{
    public class ServiceResponse<T>
    {
        public int ReturnValue { get; set; }
        public string ReturnContent { get; set; }
        public Stream ReturnStream { get; set; }
        public int NetworkOperationStatus { get; set; }
        public string NetworkOperationRequestMethod { get; set; }
        public string NetworkOperationRequestUri { get; set; }
        public string NetworkOperationRequestBody { get; set; }
        public string NetworkOperationResponseContentType { get; set; }
        public byte[] NetworkOperationResponseRawBody { get; set; }
        public string NetworkOperationResponseBody { get; set; }
        public List<ServiceError> Errors { get; set; }
        public T Data { get; set; }
        public bool Status => !Errors.Any();

        public ServiceResponse()
        {
            Errors = new List<ServiceError>();
        }

        public void AddError(Exception ex)
        {
            Errors.Add(new ServiceError(ex));
        }
        public void AddError(string errorCode)
        {
            Errors.Add(new ServiceError(errorCode));
        }
        public void AddError(string errorCode, string errorMessage)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage));
        }
        public void AddError(string errorCode, string errorMessage, string errorDetail)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage, errorDetail));
        }
        public void AddError(string errorCode, string errorMessage, string errorDetail, ServiceErrorLevel errorLevel)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage, errorDetail, errorLevel));
        }
        public void AddError(ServiceError serviceError)
        {
            Errors.Add(serviceError);
        }
        public void AddErrors(List<ServiceError> serviceErrorList)
        {
            foreach (var e in serviceErrorList)
                Errors.Add(e);
        }
    }

    public class ServiceResponse : ServiceResponse<object> { }
}