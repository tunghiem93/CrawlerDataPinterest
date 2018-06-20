using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSKeyword;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.CMSEmployees;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMS_Shared.Keyword
{
    public class CMSKeywordFactory
    {

        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create key, craw data */

        public List<CMS_KeywordModels> GetList(string groupID = "")
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get all key word */
                    var data = _db.CMS_KeyWord.Where(o => o.Status == (byte)Commons.EStatus.Active).Select(o => new CMS_KeywordModels()
                    {
                        Id = o.ID,
                        Sequence = o.Sequence,
                        KeySearch = o.KeyWord,
                        UpdatedDate = o.UpdatedDate,
                        CreatedDate = o.CreatedDate
                    }).ToList();

                    if (!string.IsNullOrEmpty(groupID)) /* filter by group ID */
                    {
                        var listKeyID = _db.CMS_R_GroupKey_KeyWord.Where(o => o.GroupKeyID == groupID && o.Status != (byte)Commons.EStatus.Deleted).Select(o => o.KeyWordID).ToList();
                        data = data.Where(o => listKeyID.Contains(o.Id)).ToList();
                    }

                    /* update quantity */
                    var listCount = _db.CMS_R_KeyWord_Pin
                        .GroupBy(o => o.KeyWordID)
                        .Select(o => new CMS_KeywordModels()
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

        public bool CreateOrUpdate(CMS_KeywordModels model, ref string msg)
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
                            var checkDup = _db.CMS_KeyWord.Where(o => o.KeyWord == model.KeySearch).FirstOrDefault();

                            if (checkDup == null)
                            {
                                /* get current seq */
                                var curSeq = _db.CMS_KeyWord.OrderByDescending(o => o.Sequence).Select(o => o.Sequence).FirstOrDefault();

                                /* add new record */
                                var newKey = new CMS_KeyWord()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    KeyWord = model.KeySearch,
                                    Status = (byte)Commons.EStatus.Active,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = model.CreatedBy,
                                    UpdatedDate = DateTime.Now,
                                    Sequence = ++curSeq,
                                };
                                _db.CMS_KeyWord.Add(newKey);
                            }
                            else if (checkDup.Status != (byte)Commons.EStatus.Active) /* re-active old key */
                            {
                                checkDup.Status = (byte)Commons.EStatus.Active;
                                checkDup.UpdatedBy = model.CreatedBy;
                                checkDup.UpdatedDate = DateTime.Now;
                            }
                            else /* duplicate key word */
                            {
                                msg = "Duplicate key word.";
                            }

                            _db.SaveChanges();
                            trans.Commit();
                        }
                        else
                        {
                            msg = "Unable to edit key word.";
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
                    var key = _db.CMS_KeyWord.Where(o => o.ID == Id).FirstOrDefault();

                    key.Status = (byte)Commons.EStatus.Deleted;
                    key.UpdatedDate = DateTime.Now;
                    key.UpdatedBy = createdBy;

                    /* delete group key */
                    var listGroupKey = _db.CMS_R_GroupKey_KeyWord.Where(o => o.KeyWordID == Id).ToList();
                    listGroupKey.ForEach(o =>
                    {
                        o.Status = (byte)Commons.EStatus.Deleted;
                        o.UpdatedDate = DateTime.Now;
                        o.UpdatedBy = createdBy;
                    });

                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Can't delete this key words.";
                result = false;
            }
            return result;
        }

        public bool DeleteAndRemoveDB(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* remove list group key*/
                    var listGroupKey = _db.CMS_R_GroupKey_KeyWord.Where(o => o.KeyWordID == Id).ToList();

                    /* remove list Key pin */
                    var listKeyPin = _db.CMS_R_KeyWord_Pin.Where(o => o.KeyWordID == Id).ToList();

                    _db.CMS_R_GroupKey_KeyWord.RemoveRange(listGroupKey);
                    _db.CMS_R_KeyWord_Pin.RemoveRange(listKeyPin);
                    _db.SaveChanges();
                    
                    /* remove list pin */
                    var listPinID = _db.CMS_R_KeyWord_Pin.Select(o => o.PinID).ToList();
                    var listPin = _db.CMS_Pin.Where(o => !listPinID.Contains(o.ID)).ToList();

                    /* remove key */
                    var key = _db.CMS_KeyWord.Where(o => o.ID == Id).FirstOrDefault();
                    
                    _db.CMS_Pin.RemoveRange(listPin);
                    _db.CMS_KeyWord.Remove(key);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Can't delete this key words.";
                result = false;
            }
            return result;
        }

        public bool AddKeyToGroup(string KeyId, string GroupKeyID, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        /* add new record */
                        var checkExist = _db.CMS_R_GroupKey_KeyWord.Where(o => o.KeyWordID == KeyId && o.GroupKeyID == GroupKeyID).FirstOrDefault();
                        if (checkExist != null)
                        {
                            if (checkExist.Status != (byte)Commons.EStatus.Active)
                            {
                                checkExist.Status = (byte)Commons.EStatus.Active;
                                checkExist.UpdatedDate = DateTime.Now;
                            }
                        }
                        else /* add new */
                        {
                            var newGroupKey = new CMS_R_GroupKey_KeyWord()
                            {
                                ID = Guid.NewGuid().ToString(),
                                GroupKeyID = GroupKeyID,
                                KeyWordID = KeyId,
                                Status = (byte)Commons.EStatus.Active,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                            };
                            _db.CMS_R_GroupKey_KeyWord.Add(newGroupKey);
                        }

                        _db.SaveChanges();
                        trans.Commit();
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
                    }
                }
            }
            return result;
        }

        public bool RemoveKeyFromGroup(string KeyId, string GroupKeyID, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        /* add new record */
                        var checkRemove = _db.CMS_R_GroupKey_KeyWord.Where(o => o.KeyWordID == KeyId && o.GroupKeyID == GroupKeyID).FirstOrDefault();
                        checkRemove.Status = (byte)Commons.EStatus.Deleted;
                        checkRemove.UpdatedDate = DateTime.Now;
                        _db.SaveChanges();
                        trans.Commit();
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
                    }
                }
            }
            return result;
        }

        public bool CrawlData(string Id, string createdBy, ref string msg)
        {
            NSLog.Logger.Info("CrawlData:", Id);
            CommonHelper.WriteLog("CrawlData: " + Id);
            CommonHelper.WriteLogs("CrawlerData : " + Id);
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get key by ID */
                    var keyWord = _db.CMS_KeyWord.Where(o => o.ID == Id).FirstOrDefault();
                    if (keyWord != null)
                    {
                        /* check time span crawl */
                        var timeSpanCrawl = DateTime.Now - keyWord.UpdatedDate;
                        if(timeSpanCrawl.Value.TotalMinutes > 5) /* 5min to crawl data again */
                        {
                            /* update crawer date */
                            var bkTime = keyWord.UpdatedDate;
                            keyWord.UpdatedDate = DateTime.Now;
                            keyWord.UpdatedBy = createdBy;
                            _db.SaveChanges();

                            /* call drawler api to crawl data */
                            var model = new CMS_CrawlerModels();
                            CMSPinFactory _fac = new CMSPinFactory();
                            CrawlerHelper.Get_Tagged_Pins(ref model, keyWord.KeyWord, Commons.PinDefault);
                            if (model != null && model.Pins != null && model.Pins.Any())
                            {
                                var listPinID = model.Pins.Select(o => o.ID).ToList();
                                foreach (var pinID in listPinID)
                                {
                                    CrawlerHelper.Get_Tagged_OrtherPins(ref model, keyWord.KeyWord, Commons.PinOrtherDefault, "", 1, pinID);
                                }
                            }
                            CommonHelper.WriteLogs("Crawler Success !!!");
                            var res = _fac.CreateOrUpdate(model.Pins, keyWord.ID, createdBy, ref msg);

                            if (res == false)
                            {
                                /* back to last crawl data */
                                keyWord.UpdatedDate = bkTime;
                                _db.SaveChanges();
                                result = false;
                            }
                            else
                            {
                                keyWord.UpdatedDate = DateTime.Now;
                                _db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Crawl data is unsuccessfully.";
                result = false;

                NSLog.Logger.Error("ErrorCrawlData: " + Id, ex);
                CommonHelper.WriteLog("ErrorCrawlData: " + Id + "\nException:"+ ex.ToString());
            }
            NSLog.Logger.Info("ResponseCrawlData: " + Id, result);
            CommonHelper.WriteLog("ResponseCrawlData: " + Id);

            return result;
        }
    }
}
