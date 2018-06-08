using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSSession
{
    public class CateSession
    {
        public List<CateSession> MainCate { get; set; }
        public List<CateSession> OrtherCate { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParrentId { get; set; }

        public CateSession()
        {
            MainCate = new List<CateSession>();
            OrtherCate = new List<CateSession>();
        }
    }
}
