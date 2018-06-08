using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity
{
    public class CMS_Context : DbContext
    {
        public CMS_Context() : base("CMS_WEB")
        {
            Database.SetInitializer<CMS_Context>(new CreateDatabaseIfNotExists<CMS_Context>());
            ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        public virtual DbSet<CMS_Employee> CMS_Employees { get; set; }
        public virtual DbSet<CMS_Categories> CMS_Categories { get; set; }
        public virtual DbSet<CMS_Products> CMS_Products { get; set; }
        public virtual DbSet<CMS_Policy> CMS_Policys { get; set; }
        public virtual DbSet<CMS_News> CMS_News { get; set; }
        public virtual DbSet<CMS_Images> CMS_Images { get; set; }
        public virtual DbSet<CMS_Customers> CMS_Customers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                                            .Where(type => !String.IsNullOrEmpty(type.Namespace))
                                            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                                            type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
