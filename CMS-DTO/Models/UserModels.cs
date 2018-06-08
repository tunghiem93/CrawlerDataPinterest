using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.Models
{
    public class UserModels
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public string ImageURL { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string CurrencySymbol { get; set; }
        public UserModels()
        {
        }
    }
}
