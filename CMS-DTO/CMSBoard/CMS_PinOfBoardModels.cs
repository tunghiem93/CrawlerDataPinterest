using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSBoard
{
    public class CMS_PinOfBoardModels
    {
        public string domain { get; set; }
        public Dictionary<string, CMS_CoverImagesModels> images { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public int repin_count { get; set; }
        public CMS_BoardModels board { get; set; }

        public CMS_PinOfBoardModels()
        {
            images = new Dictionary<string, CMS_CoverImagesModels>();
        }
    }
}
