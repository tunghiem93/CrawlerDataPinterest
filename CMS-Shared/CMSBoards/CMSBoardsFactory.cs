using CMS_DTO.CMSBoard;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSBoards
{
    public class CMSBoardsFactory
    {
        public bool CreateOrUpdate(CMSBoardModels model, ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Categories.Any(x => (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
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
                                var e = new CMS_Board() { };
                                cxt.CMS_Categories.Add(e);
                            }
                            else
                            {
                                var e = cxt.CMS_Categories.Find(model.Id);
                                if (e != null)
                                {
                                }
                            }
                            cxt.SaveChanges();
                            beginTran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "Lỗi đường truyền mạng";
                        beginTran.Rollback();
                        result = false;
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
                    var e = cxt.CMS_Categories.Find(Id);
                    cxt.CMS_Categories.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa thể loại này";
                result = false;
            }
            return result;
        }

        public CMSBoardModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSBoardModels
                    {
                        Id = x.Id,
                    }).Where(x => x.Id.Equals(Id)).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMSBoardModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSBoardModels
                    {
                        Id = x.Id,
                    }).ToList();
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
