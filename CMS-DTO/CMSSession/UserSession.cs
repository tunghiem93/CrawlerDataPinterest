using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSSession
{
    public class UserSession
    {
        public bool IsAuthenticated { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsAdminClient { get; set; }
        public string CurrencySymbol { get; set; }
        public bool RememberMe { get; set; }

        public UserSession()
        {
        }
    }
}
