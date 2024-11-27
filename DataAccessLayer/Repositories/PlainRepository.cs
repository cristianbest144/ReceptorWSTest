using DataAccessLayer.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class PlainRepository
    {
        private DbContextEF _dbContext;
        public PlainRepository()
        {
            _dbContext = new DbContextEF();
        }
        public Plain GetKey(string keyName)
        {
            try
            {
                return _dbContext.Plain
                            .SingleOrDefault(t => t.KeyName == keyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Plain Get(int id)
        {
            try
            {
                return _dbContext.Plain
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Plain> GetAll()
        {
            try
            {
                return _dbContext.Plain
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update(Plain data)
        {
            try
            {
                var dbEntity = _dbContext
                      .Plain
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.Sections = data.Sections;
                dbEntity.Name = data.Name;
                dbEntity.KeyName = data.KeyName;
                dbEntity.Description = data.Description;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Insert(Plain data)
        {
            try
            {
                var dbEntity = _dbContext.Plain.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
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
