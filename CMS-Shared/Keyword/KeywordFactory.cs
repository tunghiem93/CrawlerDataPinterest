using CMS_DTO.CMSKeyword;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.Keyword
{
    public class KeywordFactory
    {
        public List<CMS_KeywordModels> GetList()
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    var data = _db.CMS_R_KeyWord_Pin
                        .Join(_db.CMS_KeyWord, kp => kp.KeyWordID, k => k.ID, (kp, k) => new { kp, k })
                        .GroupBy(o => o.k)
                        .Select(o => new CMS_KeywordModels()
                        {
                            Id = o.Key.ID,
                            Sequence = o.Key.Sequence,
                            KeySearch = o.Key.KeyWord,
                            Quantity = o.Count(),
                        }).ToList();
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public bool CreateOrUpdate(CMS_KeywordModels model, ref string msg)
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
                    //var e = cxt.CMS_KeyWord.Find(Id);
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
