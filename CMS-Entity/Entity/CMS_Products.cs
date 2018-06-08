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
        public string ProductName { get; set; }
        public int repin_count { get; set; }
        public string Link { get; set; }
        public string Board { get; set; }
        public DateTime Created_At { get; set; }
        public string ImageURL { get; set; }
    }
}
