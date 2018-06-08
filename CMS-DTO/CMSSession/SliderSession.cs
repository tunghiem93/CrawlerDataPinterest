using CMS_DTO.CMSImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSSession
{
    public class SliderSession
    {
        public string ImageUrl { get; set; }
        public List<CMS_ImagesModels> ListImg { get; set; }
        public SliderSession()
        {
            ListImg = new List<CMS_ImagesModels>();
        }
    }
}
