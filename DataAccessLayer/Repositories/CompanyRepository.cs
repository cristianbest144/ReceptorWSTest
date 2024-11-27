using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class CompanyRepository
    {
        private DbContextEF _dbContext;
        public CompanyRepository()
        {
            _dbContext = new DbContextEF();
        }
        public Company Get(int id)
        {
            try
            {
                return _dbContext.Company
                            .SingleOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Company> GetAll()
        {
            try
            {
                return _dbContext.Company
                           .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
