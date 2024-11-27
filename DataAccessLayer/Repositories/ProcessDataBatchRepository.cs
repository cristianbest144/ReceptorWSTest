using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class ProcessDataStockRepository
    {
        private DbContextEF _dbContext;
        public ProcessDataStockRepository()
        {
            _dbContext = new DbContextEF();
        }

        public ProcessDataStock GetPlainKeyName(string plainKeyName)
        {
            try
            {
                return _dbContext.ProcessDataStock
                            .SingleOrDefault(t => t.PlainKeyName == plainKeyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ProcessDataStock GetKey(string keyName)
        {
            try
            {
                return _dbContext.ProcessDataStock
                            .SingleOrDefault(t => t.KeyName == keyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ProcessDataStock> GetStatus(string Status)
        {
            try
            {
                return _dbContext.ProcessDataStock
                            .Where(t => t.Status == Status)
                            .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ProcessDataStock Get(int id)
        {
            try
            {
                return _dbContext.ProcessDataStock
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ProcessDataStock> GetAll()
        {
            try
            {
                return _dbContext.ProcessDataStock
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Update(ProcessDataStock data)
        {
            try
            {
                var dbEntity = _dbContext
                      .ProcessDataStock
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.UpdatedDate = data.UpdatedDate;
                dbEntity.Status = data.Status;
                dbEntity.LastProcessDate = data.LastProcessDate;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Insert(ProcessDataStock data)
        {
            try
            {
                var dbEntity = _dbContext.ProcessDataStock.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Insert(List<ProcessDataStock> data)
        {
            try
            {
                var dbEntity = _dbContext.ProcessDataStock.AddRange(data);
                //_dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
