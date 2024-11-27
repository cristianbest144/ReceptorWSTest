using BusinessLogicLayer.Dto;
using DataAccessLayer.Common;
using DataAccessLayer.Entites;
using DataAccessLayer.Repository;
using Newtonsoft.Json;
using Shared;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Services.Helper
{
    public class EventlogHelper
    {
        public readonly EventLogRepository _eventLogRepository;
        public EventlogHelper()
        {
            _eventLogRepository = new EventLogRepository();
        }
        public ProcessEventLogDto ProcessEventLog<T>(T data, string type, string trackingId = null, OperationType operation = OperationType.Insert, string indexType = null, int companyId = 0)
        {
            var sr = new ProcessEventLogDto();
            sr.Code = 11;
            sr.Message = string.Empty;
            var edata = new EventLog();
            edata.Message = $"Transaction {type}";
            edata.IndexType = indexType;
            edata.CompanyId = companyId;
            if (operation == OperationType.Insert)
            {
                if (string.IsNullOrEmpty(trackingId))
                    trackingId = Guid.NewGuid().ToString();
                edata.CallStack = type;
                edata.EventCode = LogLevel.INFO.ToString();
                if (string.IsNullOrEmpty(edata.IndexType))
                    edata.IndexType = Constants.LogIndex.TRAMSACTION_INPROCESS;
                edata.PayloadIsModel = true;
                edata.Date = DateTime.Now;
                edata.TrackingId = trackingId;
                try
                {
                    edata.Payload = JsonConvert.SerializeObject(data);
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    string req;
                    using (var stringWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(stringWriter, data);
                        req = stringWriter.ToString();
                    }
                    edata.Request = req;
                    var respSr = _eventLogRepository.Save(edata);
                    if (!respSr.Status)
                    {
                        sr.Message = respSr.Errors.FirstOrDefault()?.ErrorMessage;
                        return sr;
                    }
                    sr.TrackingId = trackingId;
                    sr.Code = 1;
                }
                catch (Exception ex)
                {
                    sr.Message = ex.ToString();
                    edata.Message = sr.Message;
                    edata.IndexType = Constants.LogIndex.TRAMSACTION_ERROR;
                    _eventLogRepository.Save(edata);
                }
            }
            if (operation == OperationType.Update)
            {
                if (string.IsNullOrEmpty(edata.IndexType))
                    edata.IndexType = Constants.LogIndex.TRAMSACTION_OK;
                edata.TrackingId = trackingId;
                edata.LogIndexType = DateTime.Now.ToString();
                try
                {
                    var respSr = _eventLogRepository.UpdateTracking(edata);
                    if (!respSr.Status)
                    {
                        sr.Message = respSr.Errors.FirstOrDefault()?.ErrorMessage;
                        return sr;
                    }
                    sr.Code = 1;
                }
                catch (Exception ex)
                {
                    sr.Message = ex.ToString();
                    edata.Message = sr.Message;
                    edata.IndexType = Constants.LogIndex.TRAMSACTION_ERROR;
                    _eventLogRepository.UpdateTracking(edata);
                }
            }
            return sr;
        }
    }
}
