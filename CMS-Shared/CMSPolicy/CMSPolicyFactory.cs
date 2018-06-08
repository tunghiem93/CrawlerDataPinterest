using CMS_DTO.CMSPolicy;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSPolicy
{
    public class CMSPolicyFactory
    {
        public CMS_PolicyModels GetData()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Policys.Select(x => new CMS_PolicyModels
                    {
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    }).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public bool InsertOrUpdate(CMS_PolicyModels model, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.Id))
                        {
                            var e = new CMS_Policy
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                Description = model.Description,
                                IsActive = model.IsActive,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now
                            };
                            cxt.CMS_Policys.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_Policys.Find(model.Id);
                            e.CreatedBy = model.CreatedBy;
                            e.Description = model.Description;
                            e.IsActive = model.IsActive;
                            e.UpdatedBy = model.UpdatedBy;
                            e.UpdatedDate = DateTime.Now;
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        result = false;
                        msg = "Vui lòng kiểm tra đường truyền";
                        trans.Rollback();
                    }
                    finally
                    {
                        cxt.Dispose();
                    }
                }
            }
            return result;
        }
    }
}
