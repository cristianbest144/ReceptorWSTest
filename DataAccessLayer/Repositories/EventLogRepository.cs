using DataAccessLayer.Entites;
using Shared;
using System;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class EventLogRepository
    {
        private DbContextEF _dbContext;
        public EventLogRepository()
        {
            _dbContext = new DbContextEF();
        }
        public ServiceResponse<int> Save(EventLog data)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                var dbEntity = _dbContext.EventLog.Add(data);
                sr.ReturnValue = dbEntity.Id;
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }

            return sr;
        }

        public ServiceResponse<int> UpdateTracking(EventLog data)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                var dbEntity = _dbContext
                      .EventLog
                      .AsQueryable()
                      .SingleOrDefault(x => x.TrackingId == data.TrackingId);

                dbEntity.IndexType = data.IndexType;
                dbEntity.LogIndexType = data.LogIndexType;
                if (!string.IsNullOrEmpty(data.Message))
                    dbEntity.Message = data.Message;
                dbEntity.CompanyId = data.CompanyId;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
                sr.ReturnValue = data.Id;
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
                return sr;
            }
            return sr;
        }
    }

}
