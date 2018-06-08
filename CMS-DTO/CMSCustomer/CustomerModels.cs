using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_DTO.CMSCustomer
{
    public class CustomerModels
    {
        public string ID { get; set; }
        //[Required(ErrorMessage = "Làm ơn nhập tên đầy đủ!")]
        public string Name { get { return (this.FirstName + " " + this.LastName); } }
        [Required(ErrorMessage = "Làm ơn nhập họ!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập tên!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Làm ơn xác nhận lại mật khẩu!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Làm ơn nhập số điện thoại!")]
        public string Phone { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập địa chỉ!")]
        public string Address { get; set; }
        public bool MaritalStatus { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }

        public byte[] PictureByte { get; set; }
        public List<SelectListItem> ListMarital { get; set; }
        public List<SelectListItem> ListGender { get; set; }
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public CustomerModels()
        {
            IsActive = true;
            BirthDate = new DateTime(1990, 01, 01);
            ListMarital = new List<SelectListItem>()
            {
                new SelectListItem() {  Text = "Độc thân", Value = "False"},
                new SelectListItem() { Text = "Kết hôn", Value = "True"}
            };

            ListGender = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Nam", Value = "False"},
                new SelectListItem() {  Text = "Nữ", Value = "True"},
            };
        }
    }
}
