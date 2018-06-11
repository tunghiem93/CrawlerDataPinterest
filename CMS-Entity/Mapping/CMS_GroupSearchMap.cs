using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_GroupSearchMap : EntityTypeConfiguration<CMS_GroupSearch>
    {
        public CMS_GroupSearchMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(60);
            this.Property(x => x.KeySearch).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            this.Property(x => x.Quantity).HasColumnType("int").IsRequired();
        }
    }
}
