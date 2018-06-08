using CMS_DTO.CMSImage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_DTO.CMSProduct
{
    public class CMS_ProductsModels
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Link { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string sStatus { get; set; }

        public HttpPostedFileBase[] PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
        public string ImageURL { get; set; }
        
        public CMS_ProductsModels()
        {
        }
    }
}
