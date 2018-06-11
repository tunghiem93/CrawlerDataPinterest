using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_GroupSearch : CMS_EntityBase
    {
        public string Id { get; set; }
        public string KeySearch { get; set; }
        public int Quantity { get; set; }
    }
}
