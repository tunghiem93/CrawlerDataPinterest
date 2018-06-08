using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_EmployeeMap : EntityTypeConfiguration<CMS_Employee>
    {
        public CMS_EmployeeMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.Property(x => x.Employee_Phone).HasMaxLength(11).IsRequired().HasColumnType("varchar");
            this.Property(x => x.FirstName).HasMaxLength(50).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.LastName).HasMaxLength(20).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.Employee_IDCard).HasMaxLength(50).IsOptional().HasColumnType("varchar");
            this.Property(x => x.BirthDate).IsOptional();
            this.Property(x => x.Employee_Email).HasMaxLength(250).IsRequired().HasColumnType("varchar");
            this.Property(x => x.Employee_Address).IsOptional().HasMaxLength(250).HasColumnType("nvarchar");
            this.Property(x => x.UpdatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.Property(x => x.CreatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.Property(x => x.Password).IsRequired().HasMaxLength(250).HasColumnType("varchar");
            this.Property(x => x.ImageURL).IsOptional().HasColumnType("varchar").HasMaxLength(60);
        }
    }
}
