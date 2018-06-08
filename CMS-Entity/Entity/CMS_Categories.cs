using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Categories : CMS_EntityBase
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public virtual List<CMS_Products> Products { get; set; }
    }
}
