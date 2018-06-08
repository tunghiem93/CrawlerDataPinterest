using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_CategoriesMap : EntityTypeConfiguration<CMS_Categories>
    {
        public CMS_CategoriesMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasMaxLength(60).HasColumnType("varchar");
            this.Property(x => x.CategoryName).HasMaxLength(250).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.CategoryCode).HasMaxLength(50).IsRequired().HasColumnType("varchar");
            this.Property(x => x.UpdatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CreatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.UpdatedDate).IsOptional();
            this.Property(x => x.ParentId).HasColumnType("varchar").HasMaxLength(60).IsOptional();
        }
    }
}
