using CMS_DTO.CMSBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSGroupBoards
{
    public class CMS_GroupBoardsModels
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
        public List<CMS_GroupBoardsModels> ListGroupResult { get; set; }
        public List<CMS_BoardModels> ListBoardOnGroup { get; set; }

        public CMS_GroupBoardsModels()
        {
            ListBoardOnGroup = new List<CMS_BoardModels>();
        }
    }
}
