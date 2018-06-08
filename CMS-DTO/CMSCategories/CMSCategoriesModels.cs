using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSCategories
{
    public class CMSCategoriesModels
    {
        public string Id { get; set; }
        [Required(ErrorMessage="Vui lòng nhập tên thể loại")]
        [MaxLength(60,ErrorMessage ="Tên thể loại tối đa 250 kí tự")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập mã thể loại")]
        [MaxLength(50, ErrorMessage = "Mã thể loại tối đa 50 kí tự")]
        public string CategoryCode { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string sStatus { get; set; }
        public int NumberOfProduct { get; set; }
        public string ParentId { get; set; }
    }
}
