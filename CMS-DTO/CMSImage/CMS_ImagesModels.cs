using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS_DTO.CMSImage
{
    public class CMS_ImagesModels
    {
        public string Id { get; set; }
        public string ImageURL { get; set; }
        public string TempImageURL { get; set; }
        public int OffSet { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageName { get; set; }
        public HttpPostedFileBase PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
        public string ProductId { get; set; }
    }
}
