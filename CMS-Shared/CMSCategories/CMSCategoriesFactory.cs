using CMS_DTO.CMSCategories;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCategories
{
    public class CMSCategoriesFactory
    {
        public bool CreateOrUpdate(CMSCategoriesModels model,ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Categories.Any(x =>(x.CategoryCode.Equals(model.CategoryCode) || x.CategoryName.Equals(model.CategoryName)) && (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
                        if (_IsExits)
                        {
                            result = false;
                            msg = "Mã thể loại hoặc tên thể lại đã tồn tại";
                        } 
                        else
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                var _Id = Guid.NewGuid().ToString();
                                var e = new CMS_Categories()
                                {
                                    CategoryCode = model.CategoryCode,
                                    CategoryName = model.CategoryName,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    Description = model.Description,
                                    IsActive = model.IsActive,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = DateTime.Now,
                                    ParentId = model.ParentId,
                                    Id = _Id
                                };
                                Id = _Id;
                                cxt.CMS_Categories.Add(e);
                            }
                            else
                            {
                                var e = cxt.CMS_Categories.Find(model.Id);
                                if (e != null)
                                {
                                    e.CategoryCode = model.CategoryCode;
                                    e.CategoryName = model.CategoryName;
                                    e.Description = model.Description;
                                    e.IsActive = model.IsActive;
                                    e.UpdatedBy = model.UpdatedBy;
                                    e.UpdatedDate = DateTime.Now;
                                    e.ParentId = model.ParentId;
                                }
                            }
                            cxt.SaveChanges();
                            beginTran.Commit();
                        }
                    }
                    catch(Exception ex) {
                        msg = "Lỗi đường truyền mạng";
                        beginTran.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool Delete(string Id , ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Categories.Find(Id);
                    cxt.CMS_Categories.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                msg = "Không thể xóa thể loại này";
                result = false;
            }
            return result;
        }

        public CMSCategoriesModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSCategoriesModels
                    {
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ParentId = x.ParentId
                    }).Where(x=>x.Id.Equals(Id)).FirstOrDefault();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }

        public List<CMSCategoriesModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSCategoriesModels
                    {
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ParentId = x.ParentId
                    }).ToList();
                    return data;
                }
            }catch(Exception ex) { }
            return null;
        }
    }
}
