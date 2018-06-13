using CMS_DTO.CMSGroupSearch;
using CMS_Entity;
using CMS_Entity.Entity;
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
                    //var data = cxt.CMS_GroupSearch.Select(x => new CMS_GroupSearchModels
                    //{
                    //    Id = x.Id,
                    //    KeySearch = x.KeySearch,
                    //    Quantity = x.Quantity,
                    //    CreatedBy = x.CreatedBy,
                    //    CreatedDate = x.CreatedDate,
                    //    UpdatedBy = x.UpdatedBy,
                    //    UpdatedDate = x.UpdatedDate,
                    //}).ToList();
                    //return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public bool CreateOrUpdate(CMS_GroupSearchModels model, ref string msg)
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
                            //var _Id = Guid.NewGuid().ToString();
                            //var e = new CMS_GroupSearch
                            //{
                            //    Id = _Id,
                            //    KeySearch = model.KeySearch,
                            //    Quantity = model.Quantity,
                            //    CreatedBy = model.CreatedBy,
                            //    CreatedDate = DateTime.Now,
                            //    UpdatedBy = model.UpdatedBy,
                            //    UpdatedDate = DateTime.Now
                            //};
                            //cxt.CMS_GroupSearchs.Add(e);
                        }
                        else
                        {
                            //var e = cxt.CMS_GroupSearchs.Find(model.Id);
                            //if (e != null)
                            //{
                            //    e.KeySearch = model.KeySearch;
                            //    e.Quantity = model.Quantity;
                            //    e.CreatedBy = model.CreatedBy;
                            //    e.CreatedDate = DateTime.Now;
                            //    e.UpdatedBy = model.UpdatedBy;
                            //    e.UpdatedDate = DateTime.Now;
                            //}
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        msg = "Vui lòng kiểm tra đường truyền";
                        result = false;
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

        public bool Delete(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    //var e = cxt.CMS_GroupSearchs.Find(Id);
                    //cxt.CMS_GroupSearchs.Remove(e);
                    //cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa nhân viên này";
                result = false;
            }
            return result;
        }

        public bool Refresh(string Id, int Qty, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    //var e = cxt.CMS_GroupSearchs.Find(Id);
                    //e.Quantity = Qty;
                    //cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "";
                result = false;
            }
            return result;
        }
    }
}
