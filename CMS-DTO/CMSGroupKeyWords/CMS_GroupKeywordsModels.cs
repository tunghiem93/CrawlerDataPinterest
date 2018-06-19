using CMS_DTO.CMSKeyword;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSGroupKeywords
{
    public class CMS_GroupKeywordsModels
    {
        public string Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public byte Status { get; set; }
        public int OffSet { get; set; }
        public string StrLastUpdate { get; set; }
        public List<CMS_GroupKeywordsModels> ListGroupResult { get; set; }
        public List<CMS_KeywordModels> ListKeyOnGroup { get; set; }

        public CMS_GroupKeywordsModels()
        {
            ListKeyOnGroup = new List<CMS_KeywordModels>();
        }
    }
}
