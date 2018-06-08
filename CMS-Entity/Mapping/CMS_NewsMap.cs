using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CMS_Entity.Entity;

namespace CMS_Entity.Mapping
{
    public class CMS_NewsMap : EntityTypeConfiguration<CMS_News>
    {
        public CMS_NewsMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.Property(x => x.Short_Description).HasMaxLength(500).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.Title).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            this.Property(x => x.Description).HasColumnType("ntext").IsOptional();
            this.Property(x => x.ImageURL).HasColumnType("varchar").HasMaxLength(60).IsOptional();
            this.Property(x => x.UpdatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CreatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.UpdatedDate).IsOptional();
        }
    }
}
