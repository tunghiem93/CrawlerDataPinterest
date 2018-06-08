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
            this.Property(x => x.ProductName).HasMaxLength(500).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.repin_count).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.Link).HasMaxLength(60).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.Board).HasMaxLength(60).HasColumnType("nvarchar").IsRequired();

        }
    }
}
