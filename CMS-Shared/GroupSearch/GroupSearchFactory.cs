using CMS_DTO.CMSGroupSearch;
using CMS_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.GroupSearch
{
    public class GroupSearchFactory
    {
        public List<CMS_GroupSearchModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_GroupSearchs.Select(x => new CMS_GroupSearchModels
                    {
                        Id = x.Id,
                        Quantity = x.Quantity,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    }).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
