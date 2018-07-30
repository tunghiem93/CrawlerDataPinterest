using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace CMS_DTO.CMSBoard
{
    public class CMS_BoardModels
    {
        public int Sequence { get; set; }
        public string description { get; set; }
        public int pin_count { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }

        public string AccountID { get; set; }
        public string StrLastUpdate { get; set; }
        public bool Checked { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public List<string> pin_thumbnail_urls { get; set; }
        public Dictionary<string, CMS_CoverImagesModels> cover_images { get; set; }
        public CMS_OwnerModels owner { get; set; }

        public CMS_BoardModels()
        {
            pin_thumbnail_urls = new List<string>();
            cover_images = new Dictionary<string, CMS_CoverImagesModels>();
        }
    }

    public class CMS_CoverImagesModels
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class CMS_OwnerModels
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool domain_verified { get; set; }
        public bool is_default_image { get; set; }
        public string image_medium_url { get; set; }
        public bool explicitly_followed_by_me { get; set; }
        public string full_name { get; set; }
    }
}
