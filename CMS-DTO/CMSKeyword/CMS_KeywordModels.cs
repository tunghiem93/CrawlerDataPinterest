using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSKeyword
{
    public class CMS_KeywordModels
    {
        public string Id { get; set; }
        public int Sequence { get; set; }
        public string KeySearch { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
        public int OffSet { get; set; }
        public string StrLastUpdate { get; set; }

        public List<CMS_KeywordModels> ListKeyResult { get; set; }
        public CMS_KeywordModels()
        {
            ListKeyResult = new List<CMS_KeywordModels>();
        }
    }
}
