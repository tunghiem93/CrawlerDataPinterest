using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSKeyword;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.CMSEmployees;
using CMS_Shared.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CMS_Shared.Keyword
{
    public class CMSKeywordFactory
    {

        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create key, craw data */
        private static Semaphore m_SemaphoreCrawlAll = new Semaphore(1, 1); /* semaphore for create key, craw data */

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
                            model.KeySearch = model.KeySearch.Trim();
                            var checkDup = _db.CMS_KeyWord.Where(o => o.KeyWord == model.KeySearch).FirstOrDefault();

                            if (checkDup == null)
                            {
                                /* get current seq */
                                var curSeq = _db.CMS_KeyWord.OrderByDescending(o => o.Sequence).Select(o => o.Sequence).FirstOrDefault();

                                /* add new record */
                                var dateTimeNow = DateTime.Now;
                                var newKey = new CMS_KeyWord()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    KeyWord = model.KeySearch,
                                    Status = (byte)Commons.EStatus.Active,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = dateTimeNow,
                                    UpdatedBy = model.CreatedBy,
                                    UpdatedDate = dateTimeNow,
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
                    _db.Database.CommandTimeout = 500;

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

        public bool DeleteAndRemoveDBCommand(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    _db.Database.CommandTimeout = 500;

                    /* delete keyword_pin */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_R_KeyWord_Pin where  KeyWordID = \'" + Id + "\'"
                        );

                    /* delete pin */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_Pin where ID not in (select PinID from CMS_R_KeyWord_Pin)"
                        );

                    /* delete group_key */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_R_GroupKey_KeyWord where KeyWordID = \'" + Id + "\'"
                        );

                    /* delete key */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_KeyWord where ID = \'" + Id + "\'"
                        );
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
            NSLog.Logger.Info("CrawlData: " + Id);
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
                        if (timeSpanCrawl.Value.TotalMinutes > 5 || keyWord.UpdatedDate == keyWord.CreatedDate) /* 5min to crawl data again */
                        {
                            /* update crawer date */
                            var bkTime = keyWord.UpdatedDate;
                            keyWord.UpdatedDate = DateTime.Now;
                            keyWord.UpdatedBy = createdBy;
                            keyWord.KeyWord = keyWord.KeyWord.Trim();
                            _db.SaveChanges();
                            
                            /* cookies 
                                User: chivietarsenal@gmail.com
                                Pass: pitool.org79
                            */
                            CrawlerHelper._Cookies = "_b = \"AS+B1gn0GdpGgLQl83JubKX1bG19kiuUUvX8lnvITKDHNq2tJcgqXNIQ0cLN+kjq4KM=\"; _pinterest_pfob = enabled; _ga = GA1.2.229901352.1528170174; pnodepath = \"/pin4\"; fba = True; G_ENABLED_IDPS = google; bei = false; logged_out = True; sessionFunnelEventLogged = 1; cm_sub = denied; _auth = 1; csrftoken = fkrSitmDb4vW2kT1G3GfOkcC8mPvl0kV; _pinterest_sess = \"TWc9PSZWaE0xeDZOVm4yL3Yva0VSazRkRjlHR013bk9mdVJBcU9zVEtEOUhXVjhKSFZmZUEreWJiNDYrV3FubVRoVzdqdDF0dmtDcXErcFF6MmlXQUQ3RDVzWERCWTZYZUt4eXMzemkvOGlXRFZQT1J6MjkwampOZlVJUFEvTnNkTUZYMkJ3dGxPTTRKaVIwdGNJY2h1MUhaSHlFT3djd0huNHE0YmtiTTBZR3dVTVB3d0RyYVE4UC8rMjZCYWo2eTJLNGJVSHR6KzRENjlWVE0rNFMxNWdGMUtVL0VtL2RDZktiUFg3M1Y2Z2dEbllPeUxFR3FOdEd6SUJSRTlBMWs1YkJnbTBlWHhwcC9pMmlqRmoydlh0V2VQSGYvYk1zeXlSM2dIU1dmUXIyRWVxWVBPdTYzbHFjcVhYRWRBT0FTQ3VBNmdWMm5QUlREZDdSY2ZQeE1NWklqSUZxNDllVHF1WUVzRFRrRjBXQnZCMVBGTlYxT0UzM1daeHFOUnBBTzliMzFJdmovQ1hQR2Vvc1pkTHNxL1FjT3FrWllTR1d6VHFrd2g5cFBFMmswM3dIa0dOOHVCbGd6aVlKUkJlZlZNeWVyRTBYREcrQVFlUTdRc1NqMlFlQ3RvaWlZMjJXZ1RURmIxNDA2d2JTODRGNk9BYWpoRzVJTUhLMkJ4UDJGb0NmN0NOQXpmZ0FoR08xcElmWmh5S29OeGRadFpDVWR1RGw3ZzZGRS81SlU4UlhSUVlIWm4wRzRJMGFVaTQzdGI3T2ovSCtHR2ZSWlk0M1RCN2JXSmZJRFdQUUpZWVpRMW5ta0pMbXgwT2NZckZJcHg0RTJrTjJlZWJIdXFSdkdJTWNXc2d3NHpXdzFTRGhKVkN4YmY4SCtJaTdSQSt0K2dhc1VDc0tkNnJIeVFhb3BHeDd6OUwvamZsanRKV0ZYNGFmZWFQNGlqNFVqekVFcGUreHU4UGVqZXRuMFVDNE1QbkFuWnJ6YzNjMTF3dVNZUHJ2MjBwMi8xeXNwbnczMlpSa3cvbzVPQUhQSyswNlU4Y2JQaThxNWN1NWtHVm83SWc0YjJVVW1tUWZYcHpWR2RCYS8wRE0yb2RtNUs0NzRteFp4JjVhOXZDbjB5RGtxL1lROE5WOVNDMjB4c1dMND0=\"";
                            var searchStr = HttpUtility.UrlEncode(keyWord.KeyWord);

                            /* get first class result */
                            var model = new CMS_CrawlerModels();
                            CMSPinFactory _fac = new CMSPinFactory();
                            CrawlerHelper.Get_Tagged_Pins(ref model, searchStr, Commons.PinDefault);
                            if (model != null && model.Pins != null && model.Pins.Any())
                            {
                                /* get second class result */
                                var listPinID = model.Pins.Select(o => o.ID).ToList();
                                Parallel.ForEach(listPinID, pinID =>
                                {
                                    CrawlerHelper.Get_Tagged_OrtherPins(ref model, searchStr, Commons.PinOrtherDefault, "", 1, pinID);
                                });
                            }

                            /* create or update pin */
                            var res = _fac.CreateOrUpdate(model.Pins, keyWord.ID, createdBy, ref msg);

                            if (res == false)
                            {
                                /* back to last crawl data */
                                //keyWord.UpdatedDate = bkTime;
                                //_db.SaveChanges();
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

                NSLog.Logger.Info("ResponseCrawlData: " + Id, result);
            }
            catch (Exception ex)
            {
                msg = "Crawl data is unsuccessfully.";
                result = false;
                LogHelper.WriteLogs("ErrorCrawlData: " + Id, JsonConvert.SerializeObject(ex));
                NSLog.Logger.Error("ErrorCrawlData: " + Id, ex);
            }

            return result;
        }

        public bool CrawlAllKeyWords(string createdBy, ref string msg)
        {
            NSLog.Logger.Info("CrawlAllKeyWords");
            LogHelper.WriteLogs("CrawllAllKeyWords", "");
            var result = true;
            try
            {
                //new Thread(() => { var auto = AutoSingleton.Instance; }).Start();

                m_SemaphoreCrawlAll.WaitOne();
                using (var _db = new CMS_Context())
                {
                    var keyWords = _db.CMS_KeyWord.Where(o => o.Status == (byte)Commons.EStatus.Active).OrderBy(o => o.Sequence).ToList();
                    foreach (var key in keyWords)
                    {
                        LogHelper.WriteLogs("KeyWords", key.Sequence.ToString());
                        CrawlData(key.ID, createdBy, ref msg);
                    }
                }
                LogHelper.WriteLogs("CrawlAllKeyWords", "finish");

                NSLog.Logger.Info("ResponseCrawlAllKeyWords", result);
            }
            catch (Exception ex)
            {
                msg = "Crawl data is unsuccessfully.";
                result = false;
                NSLog.Logger.Error("ErrorCrawlAllKeyWords", ex);
            }
            finally
            {
                m_SemaphoreCrawlAll.Release();
            };

            return result;
        }

        public bool DeleteAll(string createdBy, ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    var keys = _db.CMS_KeyWord.Where(o => o.Status == (byte)Commons.EStatus.Active).ToList();
                    var keyIDs = keys.Select(o => o.ID).ToList();
                    var keyGroupKeyDB = _db.CMS_R_GroupKey_KeyWord.Where(o => keyIDs.Contains(o.KeyWordID)).ToList();

                    /* delete key */
                    keys.ForEach(key =>
                    {
                        key.Status = (byte)Commons.EStatus.Deleted;
                        key.UpdatedDate = DateTime.Now;
                        key.UpdatedBy = createdBy;
                    });

                    /* delete group key */
                    keyGroupKeyDB.ForEach(o =>
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

        public bool DeleteAndRemoveDBAll(ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    var keys = _db.CMS_KeyWord.Select(o => o.ID).ToList();
                    foreach (var keyID in keys)
                    {
                        DeleteAndRemoveDBCommand(keyID, ref msg);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Can't delete data.";
                result = false;
            }
            return result;
        }
    }
}
