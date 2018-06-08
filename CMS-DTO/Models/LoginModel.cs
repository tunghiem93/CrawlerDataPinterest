using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.Models
{
    public class LoginRequestModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public string LanguageId { get; set; }

        public LoginRequestModel()
        {
        }
    }
    public class LoginResponseModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeImageURL { get; set; }
        public bool IsSupperAdmin { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }        

        public LoginResponseModel()
        {
            
        }
    }
}
