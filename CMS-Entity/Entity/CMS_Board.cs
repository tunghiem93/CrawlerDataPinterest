using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Board
    {
        public string Id { get; set; }
        public string BoardName { get; set; }
        public int repin_count { get; set; }
        public string Link { get; set; }
        public DateTime Created_At { get; set; }
        public string ImageURL { get; set; }
    }
}
