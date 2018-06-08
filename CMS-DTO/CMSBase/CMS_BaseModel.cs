using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSBase
{
    class CMS_BaseModel
    {
    }

    public class CategoryByCategory
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<CategoryChildren> children { get; set; }
        public CategoryByCategory()
        {
            children = new List<CategoryChildren>();
        }
    }
    public class CategoryChildren
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
