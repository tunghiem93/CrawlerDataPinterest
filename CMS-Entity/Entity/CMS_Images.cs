using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Images : CMS_EntityBase
    {
        public string Id { get; set; }
        public string ImageURL { get; set; }
        public string ProductId { get; set; }

        public virtual CMS_Products Product { get; set; }
    }
}
