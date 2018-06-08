using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CMS_Entity.Entity;

namespace CMS_Entity.Mapping
{
    public class CMS_CustomerMap : EntityTypeConfiguration<CMS_Customers>
    {
        public CMS_CustomerMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasMaxLength(60).HasColumnType("varchar").IsRequired();
            this.Property(x => x.Address).HasMaxLength(250).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.CompanyName).HasMaxLength(250).HasColumnType("nvarchar").IsOptional();
            this.Property(x => x.CreatedBy).HasColumnType("varchar").HasMaxLength(60).IsOptional();
            this.Property(x => x.FirstName).HasMaxLength(50).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.LastName).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            this.Property(x => x.Password).HasMaxLength(250).HasColumnType("varchar").IsRequired();
            this.Property(x => x.Phone).HasColumnType("varchar").HasMaxLength(15).IsOptional();
            this.Property(x => x.Street).HasMaxLength(250).HasColumnType("nvarchar").IsOptional();
            this.Property(x => x.UpdatedBy).HasColumnType("varchar").HasMaxLength(60).IsOptional();
        }
    }
}
