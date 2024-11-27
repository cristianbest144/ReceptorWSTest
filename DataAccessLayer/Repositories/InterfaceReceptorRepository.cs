using DataAccessLayer.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class InterfaceReceptorRepository
    {
        private DbContextEF _dbContext;
        public InterfaceReceptorRepository()
        {
            _dbContext = new DbContextEF();
        }
        public ServiceResponse<InterfaceReceptor> Get(int id)
        {
            var sr = new ServiceResponse<InterfaceReceptor>();
            try
            {
                sr.Data = _dbContext.InterfaceReceptor
                            .SingleOrDefault(t => t.Id == id);
                if (!sr.Data.IsStatus)
                    sr.AddError(sr.Data.MessageLog);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }
            return sr;
        }
        public ServiceResponse<InterfaceReceptor> GetKeyAndCompany(string keyName, int companyId, string type = "RECEIVER")
        {
            var sr = new ServiceResponse<InterfaceReceptor>();
            try
            {
                sr.Data = _dbContext.InterfaceReceptor
                            .SingleOrDefault(t => t.KeyName == keyName && t.CompanyId == companyId && t.Type == type);
                if (!sr.Data.IsStatus)
                    sr.AddError(sr.Data.MessageLog);
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }
            return sr;
        }
        public ServiceResponse<List<InterfaceReceptor>> GetByCompanyIdAll(int companyId)
        {
            var sr = new ServiceResponse<List<InterfaceReceptor>>();
            try
            {
                sr.Data = _dbContext.InterfaceReceptor
                           .Where(t => t.CompanyId == companyId)
                           .ToList();
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }
            return sr;
        }
        public ServiceResponse Update(InterfaceReceptor data)
        {
            var sr = new ServiceResponse();
            try
            {
                var dbEntity = _dbContext
                      .InterfaceReceptor
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.UpdatedDate = DateTime.Now;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }
            return sr;
        }
        public ServiceResponse Insert(InterfaceReceptor data)
        {
            var sr = new ServiceResponse();
            try
            {
                var dbEntity = _dbContext.InterfaceReceptor.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                sr.AddError(ex);
            }
            return sr;
        }
    }
}
