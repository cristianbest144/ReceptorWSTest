using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository
{
    public class ProcessConfOrdenFabricacionRepository
    {
        private DbContextEF _dbContext;
        public ProcessConfOrdenFabricacionRepository()
        {
            _dbContext = new DbContextEF();
        }

        public List<ProcessConfOrdenFabricacion> GetStatus(string Status)
        {
            try
            {
                return _dbContext.ProcessConfOrdenFabricacion
                            .Where(t => t.Status == Status)
                            .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ProcessConfOrdenFabricacion Get(int id)
        {
            try
            {
                return _dbContext.ProcessConfOrdenFabricacion
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ProcessConfOrdenFabricacion> GetAll()
        {
            try
            {
                return _dbContext.ProcessConfOrdenFabricacion
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Update(ProcessConfOrdenFabricacion data)
        {
            try
            {
                var dbEntity = _dbContext
                      .ProcessConfOrdenFabricacion
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.UpdatedDate = data.UpdatedDate;
                dbEntity.Status = data.Status;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Insert(ProcessConfOrdenFabricacion data)
        {
            try
            {
                var dbEntity = _dbContext.ProcessConfOrdenFabricacion.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Insert(List<ProcessConfOrdenFabricacion> data)
        {
            try
            {
                var dbEntity = _dbContext.ProcessConfOrdenFabricacion.AddRange(data);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<ProcessConfOrdenFabricacion> GetAll(Expression<Func<ProcessConfOrdenFabricacion, bool>> condition)
        {
            try
            {
                return _dbContext.ProcessConfOrdenFabricacion
                           .Where(condition)
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
