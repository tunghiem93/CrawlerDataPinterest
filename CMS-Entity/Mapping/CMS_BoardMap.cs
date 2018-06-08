using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_BoardMap : EntityTypeConfiguration<CMS_Board>
    {
        public CMS_BoardMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.Property(x => x.BoardName).HasMaxLength(500).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.repin_count).HasColumnType("nvarchar").IsRequired();
            this.Property(x => x.Link).HasMaxLength(60).HasColumnType("nvarchar").IsRequired();
        }
    }
}
