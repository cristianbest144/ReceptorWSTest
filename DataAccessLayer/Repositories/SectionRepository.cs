using DataAccessLayer.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class SectionRepository
    {
        private DbContextEF _dbContext;
        public SectionRepository()
        {
            _dbContext = new DbContextEF();
        }
        public PlainSection GetKey(string keyName)
        {
            try
            {
                return _dbContext.PlainSection
                            .SingleOrDefault(t => t.KeyName == keyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public PlainSection Get(int id)
        {
            try
            {
                return _dbContext.PlainSection
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PlainSection> GetAll()
        {
            try
            {
                return _dbContext.PlainSection
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update(PlainSection data)
        {
            try
            {
                var dbEntity = _dbContext
                      .PlainSection
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.KeyName = data.KeyName;
                dbEntity.Name = data.Name;
                dbEntity.Order = data.Order;
                dbEntity.AllowMultipleRows = data.AllowMultipleRows;
                dbEntity.Plain = data.Plain;
                dbEntity.Fields = data.Fields;
                dbEntity.IsRequired = data.IsRequired;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Insert(PlainSection data)
        {
            try
            {
                var dbEntity = _dbContext.PlainSection.Add(data);
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
