using DataAccessLayer.Entites;
using Shared;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class DbContextEF : DbContext
    {
        public DbContextEF() : base(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTIONSTRINGNAME].ToString())
        {
            Database.SetInitializer<DbContextEF>(null); // Desactiva la verificación del modelo
        }
        public DbSet<EventLog> EventLog { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<CustomizationUI> CustomizationUI { get; set; }
        public DbSet<InputParameter> InputParameter { get; set; }
        public DbSet<ParameterOption> ParameterOption { get; set; }
        public DbSet<Plain> Plain { get; set; }
        public DbSet<PlainField> PlainField { get; set; }
        public DbSet<PlainSection> PlainSection { get; set; }
        public DbSet<ProcessDataStock> ProcessDataStock { get; set; }
        public DbSet<InterfaceReceptor> InterfaceReceptor { get; set; }
        public DbSet<ProcessConfOrdenFabricacion> ProcessConfOrdenFabricacion { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Aquí puedes configurar el mapeo de tus entidades
            // Ejemplo:
            // modelBuilder.Entity<MyEntity>().ToTable("MyEntityTableName");
        }
        private void FixEfProviderServicesProblem()
        {
            // The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            // for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            // Make sure the provider assembly is available to the running application. 
            // See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public int SaveChanges()
        {
            return   base.SaveChanges();
        }
    }
}
