using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Products : CMS_EntityBase
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }

        public virtual CMS_Categories Category { get; set; }
        public virtual List<CMS_Images> Images { get; set; }
    }
}
