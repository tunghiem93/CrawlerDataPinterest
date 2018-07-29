using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSAccount
{
    public class CMS_AccountModels
    {
        public string Id { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Cookies { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public CMS_AccountModels()
        {
            IsActive = true;
        }
    }    
}
