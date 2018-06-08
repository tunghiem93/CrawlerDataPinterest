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
        [Required(ErrorMessage ="Vui lòng nhập mã sản phẩm")]
        [MaxLength(50,ErrorMessage ="Mã sản phẩm tối đa 50 kí tự")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [MaxLength(250,ErrorMessage ="Tên sản phẩm tối đa 250 kí tự")]
        public string ProductName { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập giá sản phẩm")]
        [Range(0,Int64.MaxValue,ErrorMessage ="Giá sản phẩm phải lớn hơn 0")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage ="Vui lòng chọn thể loại")]
        public string CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        public string CategoryName { get; set; }
        public string sStatus { get; set; }

        public HttpPostedFileBase[] PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
        public string ImageURL { get; set; }

        public List<CMS_ImagesModels> ListImages { get; set; }

        public CMS_ProductsModels()
        {
            ListImages = new List<CMS_ImagesModels>();
        }
    }
}
