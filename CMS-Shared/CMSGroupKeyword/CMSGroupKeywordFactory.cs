using CMS_DTO.CMSGroupKeywords;
using CMS_Entity.Entity;
using CMS_Shared.Keyword;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMS_Shared.CMSGroupKeywords
{
    public class CMSGroupKeywordsFactory
    {
        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create key, craw data */

        public List<CMS_GroupKeywordsModels> GetList()
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get all key word */
                    var data = _db.CMS_GroupKey.Where(o => o.Status == (byte)Commons.EStatus.Active).Select(o => new CMS_GroupKeywordsModels()
                    {
                        Id = o.ID,
                        Sequence = o.Sequence,
                        Name = o.Name,
                        UpdatedDate = o.UpdatedDate,
                        CreatedDate = o.CreatedDate
                    }).ToList();

                    /* update quantity */
                    var listCount = _db.CMS_R_GroupKey_KeyWord.Where(o => o.Status != (byte)Commons.EStatus.Deleted)
                        .Join(_db.CMS_R_KeyWord_Pin.Where(o=> o.Status != (byte)Commons.EStatus.Deleted), g=> g.KeyWordID, k=>k.KeyWordID, (g,k)=> new { g, k })
                        .GroupBy(o => o.g.GroupKeyID)
                        .Select(o => new
                        {
                            Id = o.Key,
                            Quantity = o.Count(),
                        }).ToList();

                    data.ForEach(o =>
                    {
                        o.Quantity = listCount.Where(c => c.Id == o.Id).Select(c => c.Quantity).FirstOrDefault();
                        o.StrLastUpdate = CommonHelper.GetDurationFromNow(o.UpdatedDate);
                    });

                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public CMS_GroupKeywordsModels GetDetail(string id)
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get all key word */
                    var data = _db.CMS_GroupKey.Where(o => o.Status == (byte)Commons.EStatus.Active && o.ID == id).Select(o => new CMS_GroupKeywordsModels()
                    {
                        Id = o.ID,
                        Sequence = o.Sequence,
                        Name = o.Name,
                        UpdatedDate = o.UpdatedDate,
                        CreatedDate = o.CreatedDate
                    }).FirstOrDefault();

                    /* update quantity */
                    CMSKeywordFactory keyFac = new CMSKeywordFactory();
                    data.ListKeyOnGroup = keyFac.GetList(id);

                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public bool CreateOrUpdate(CMS_GroupKeywordsModels model, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    m_Semaphore.WaitOne();
                    try
                    {
                        if (string.IsNullOrEmpty(model.Id))
                        {
                            /* check dup old key */
                            var checkDup = _db.CMS_GroupKey.Where(o => o.Name.Trim() == model.Name.Trim()).FirstOrDefault();

                            if (checkDup == null)
                            {
                                /* get current seq */
                                var curSeq = _db.CMS_GroupKey.OrderByDescending(o => o.Sequence).Select(o => o.Sequence).FirstOrDefault();

                                /* add new record */
                                var newGroup = new CMS_GroupKey()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    Name = model.Name,
                                    Status = (byte)Commons.EStatus.Active,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = model.CreatedBy,
                                    UpdatedDate = DateTime.Now,
                                    Sequence = ++curSeq,
                                };
                                _db.CMS_GroupKey.Add(newGroup);
                            }
                            else if (checkDup.Status != (byte)Commons.EStatus.Active) /* re-active old key */
                            {
                                checkDup.Status = (byte)Commons.EStatus.Active;
                                checkDup.UpdatedBy = model.CreatedBy;
                                checkDup.UpdatedDate = DateTime.Now;
                            }
                            else /* duplicate group */
                            {
                                msg = "Duplicate group.";
                            }

                            _db.SaveChanges();
                            trans.Commit();
                        }
                        else
                        {
                            msg = "Unable to edit group.";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "Check connection, please!";
                        result = false;
                        trans.Rollback();
                    }
                    finally
                    {
                        _db.Dispose();
                        m_Semaphore.Release();
                    }
                }
            }
            return result;
        }

        public bool Delete(string Id, string createdBy, ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    var group = _db.CMS_GroupKey.Where(o => o.ID == Id).FirstOrDefault();

                    if (group != null)
                    {
                        group.Status = (byte)Commons.EStatus.Deleted;
                        group.UpdatedDate = DateTime.Now;
                        group.UpdatedBy = createdBy;

                        /* loop list group key */
                        var listGroupKey = _db.CMS_R_GroupKey_KeyWord.Where(o => o.GroupKeyID == group.ID).ToList();
                        listGroupKey.ForEach(o =>
                        {
                            o.Status = (byte)Commons.EStatus.Deleted;
                            o.UpdatedDate = DateTime.Now;
                            o.UpdatedBy = createdBy;
                        });

                        _db.SaveChanges();
                    }
                    else
                    {
                        result = false;
                        msg = "Unable to find group.";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Can't delete this group.";
                result = false;
            }
            return result;
        }

    }
}
