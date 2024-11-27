using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class ConfigurationRepository
    {
        private DbContextEF _dbContext;
        public ConfigurationRepository()
        {
            _dbContext = new DbContextEF();
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

        public Configuration Get(string name,int cia)
        {
            try
            {
                return _dbContext.Configuration
                            .SingleOrDefault(t => t.Name == name && t.Id_Cia == cia);
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
    }
}
