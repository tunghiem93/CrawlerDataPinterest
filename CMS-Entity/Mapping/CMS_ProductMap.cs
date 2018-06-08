using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_ProductMap : EntityTypeConfiguration<CMS_Products>
    {
        public CMS_ProductMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.Property(x => x.ProductCode).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            this.Property(x => x.ProductName).HasMaxLength(250).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.ProductPrice).HasColumnType("decimal").IsOptional();
            this.Property(x => x.UpdatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.Property(x => x.CreatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);

        }
    }
}
