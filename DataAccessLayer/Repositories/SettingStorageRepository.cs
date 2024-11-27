using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class SettingStorageRepository
    {
        private DbContextEF _dbContext;
        public SettingStorageRepository()
        {
            _dbContext = new DbContextEF();
        }
        public Configuration GetKeyAndCompany(string keyName, long companyId)
        {
            try
            {
                return _dbContext.Configuration
                            .SingleOrDefault(t => t.Name == keyName && t.Id_Cia == companyId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Configuration GetKey(string keyName)
        {
            try
            {
                return _dbContext.Configuration
                            .SingleOrDefault(t => t.Name == keyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Configuration Get(int id)
        {
            try
            {
                return _dbContext.Configuration
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Configuration> GetAll()
        {
            try
            {
                return _dbContext.Configuration
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Update(Configuration data)
        {
            try
            {
                var dbEntity = _dbContext
                      .Configuration
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.Name = data.Name;
                dbEntity.Value = data.Value;
                dbEntity.Id_Cia = data.Id_Cia;
                dbEntity.Company = data.Company;
                dbEntity.TenantId = data.TenantId;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Insert(Configuration data)
        {
            try
            {
                var dbEntity = _dbContext.Configuration.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<Configuration> GetKeyByCompany(string keyName, long companyId)
        {
            try
            {
                return _dbContext.Configuration
                            .Where(t => t.Name == keyName && t.Id_Cia == companyId)
                            .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
