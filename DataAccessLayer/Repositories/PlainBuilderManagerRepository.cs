using Dapper;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.Repository
{
    public class PlainBuilderManagerRepository
    {
        public ServiceResponse<IEnumerable<T>> GetQuery<T>(string query, string server, string database, string userName, string password)
        {
            var data = new ServiceResponse<IEnumerable<T>>();
            try
            {
                string conexionString = $"Data Source={server};Initial Catalog={database};User ID={userName};Password={password}";
                using (var connection = new SqlConnection(conexionString))
                {
                    data.Data = connection.Query<T>(query);
                }
            }
            catch (Exception ex)
            {
                data.AddError(ex);
            }

            return data;
        }
    }
}
