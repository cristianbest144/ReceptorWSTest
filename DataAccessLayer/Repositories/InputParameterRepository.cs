using DataAccessLayer.Entites;
using Shared.Network.MapperModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class InputParameterRepository
    {
        private DbContextEF _dbContext;
        public InputParameterRepository()
        {
            _dbContext = new DbContextEF();
        }

        public InputParameter GetKey(string keyName)
        {
            try
            {
                return _dbContext.InputParameter
                            .SingleOrDefault(t => t.PlainKeyName == keyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public InputParameter Get(int id)
        {
            try
            {
                return _dbContext.InputParameter
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<InputParameter> GetAll()
        {
            try
            {
                return _dbContext.InputParameter
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update(InputParameter data)
        {
            try
            {
                var dbEntity = _dbContext
                      .InputParameter
                      .AsQueryable()
                      .SingleOrDefault(x => x.Id == data.Id);

                dbEntity.ParameterOption = data.ParameterOption;
                dbEntity.PlainKeyName = data.PlainKeyName;
                dbEntity.SupplierItemPlan = data.SupplierItemPlan;
                dbEntity.SubcategoryItemPlan = data.SubcategoryItemPlan;
                dbEntity.AppliedMarginItem = data.AppliedMarginItem;
                dbEntity.CategoryItemPlan = data.CategoryItemPlan;
                dbEntity.Company = data.Company;
                dbEntity.Consecutive = data.Consecutive;
                dbEntity.CostOverrunItem = data.CostOverrunItem;
                dbEntity.DocumentType = data.DocumentType;
                //dbEntity.FilterAdditionals = data.FilterAdditionals;
                _dbContext.SaveChanges();
                _dbContext.Entry(dbEntity).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Insert(InputParameter data)
        {
            try
            {
                var dbEntity = _dbContext.InputParameter.Add(data);
                _dbContext.Entry(dbEntity).State = EntityState.Added;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<InputParameter> GetByCompanyAll(int companyId)
        {
            try
            {
                return _dbContext.InputParameter
                           .Where(t=> t.CompanyId == companyId)
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
