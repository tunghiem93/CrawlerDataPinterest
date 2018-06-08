using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CMS_Entity.Entity;

namespace CMS_Entity.Mapping
{
    public class CMS_ImagesMap : EntityTypeConfiguration<CMS_Images>
    {
        public CMS_ImagesMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.HasOptional(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductId);
            this.Property(x => x.ProductId).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.Property(x => x.ImageURL).HasColumnType("varchar").HasMaxLength(60).IsOptional();
            this.Property(x => x.UpdatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
            this.Property(x => x.CreatedBy).HasMaxLength(60).HasColumnType("varchar").IsOptional();
        }
    }
}
