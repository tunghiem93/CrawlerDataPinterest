using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_DTO.CMSNews
{
    public class CMS_NewsModels
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tiêu đề bài viết")]
        [MaxLength(150,ErrorMessage ="Tiêu đề bài viết tối đa 150 kí tự")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mô tả bài viết")]
        [MaxLength(500,ErrorMessage ="Mô tả bài viết tối đa 500 kí tự")]
        public string Short_Description { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string sStatus { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }

        public byte[] PictureByte { get; set; }
        public string ImageURL { get; set; }

        
    }

    public class CMS_NewsViewModel
    {
        public List<CMS_NewsModels> ListNews { get; set; }
        public List<CMS_NewsModels> ListNewsNew { get; set; }
        public CMS_NewsModels CMS_News { get; set; }
        public CMS_NewsViewModel()
        {
            ListNews = new List<CMS_NewsModels>();
            ListNewsNew = new List<CMS_NewsModels>();
            CMS_News = new CMS_NewsModels();
        }
    }
}
