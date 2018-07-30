using CMS_DTO.CMSBoard;
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

namespace CMS_Shared.CMSBoard
{
    public class CMSBoardFactory
    {

        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create key, craw data */
        private static Semaphore m_SemaphoreCrawlAll = new Semaphore(1, 1); /* semaphore for create key, craw data */

        public List<CMS_BoardModels> GetList(string groupID = "")
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get all key word */
                    var data = _db.CMS_Board.Where(o => o.Status == (byte)Commons.EStatus.Active).Select(o => new CMS_BoardModels()
                    {
                       id = o.ID,
                       name = o.BoardName,
                       type = o.Type,
                       description = o.Description,
                       pin_count = o.Pin_count??0,
                       Sequence = o.Sequence.Value,
                       owner = new CMS_OwnerModels()
                       {
                           username = o.OwnerName,
                       }
                    }).ToList();

                    if (!string.IsNullOrEmpty(groupID)) /* filter by group ID */
                    {
                        var listBoardID = _db.CMS_R_GroupBoard_Board.Where(o => o.GroupBoardID == groupID && o.Status != (byte)Commons.EStatus.Deleted).Select(o => o.BoardID).ToList();
                        data = data.Where(o => listBoardID.Contains(o.id)).ToList();
                    }

                    /* update quantity */
                    var listCount = _db.CMS_R_Board_Pin
                        .GroupBy(o => o.BoardID)
                        .Select(o => new CMS_KeywordModels()
                        {
                            Id = o.Key,
                            Quantity = o.Count(),
                        }).ToList();

                    data.ForEach(o =>
                    {
                        o.pin_count = listCount.Where(c => c.Id == o.id).Select(c => c.Quantity).FirstOrDefault();
                        //o.StrLastUpdate = CommonHelper.GetDurationFromNow(o.UpdatedDate);
                    });
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public bool CreateOrUpdate(CMS_BoardModels model, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    m_Semaphore.WaitOne();
                    try
                    {
                        if (string.IsNullOrEmpty(model.id))
                        {
                            /* check dup old key */
                            var checkDup = _db.CMS_Board.Where(o => o.ID == model.id).FirstOrDefault();

                            if (checkDup == null)
                            {
                                /* get current seq */
                                var curSeq = _db.CMS_Board.OrderByDescending(o => o.Sequence).Select(o => o.Sequence).FirstOrDefault();

                                /* add new record */
                                var dateTimeNow = DateTime.Now;
                                var newKey = new CMS_Board()
                                {
                                    ID = model.id,
                                    BoardName = model.name,
                                    OwnerName = model.owner != null ? model.owner.username:"",
                                    Pin_count = model.pin_count,
                                    Description = model.description,
                                    Sequence = ++curSeq,


                                    Status = (byte)Commons.EStatus.Active,
                                    //CreatedBy = model.CreatedBy,
                                    CreatedDate = dateTimeNow,
                                    //UpdatedBy = model.CreatedBy,
                                    UpdatedDate = dateTimeNow,

                                };
                                _db.CMS_Board.Add(newKey);
                            }
                            else if (checkDup.Status != (byte)Commons.EStatus.Active) /* re-active old key */
                            {
                                checkDup.Status = (byte)Commons.EStatus.Active;
                                //checkDup.UpdatedBy = model.by;
                                checkDup.UpdatedDate = DateTime.Now;
                            }
                            else /* duplicate key word */
                            {
                                msg = "Duplicate board.";
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
                    var key = _db.CMS_Board.Where(o => o.ID == Id).FirstOrDefault();

                    key.Status = (byte)Commons.EStatus.Deleted;
                    key.UpdatedDate = DateTime.Now;
                    key.UpdatedBy = createdBy;

                    /* delete group board */
                    var listGroupKey = _db.CMS_R_GroupBoard_Board.Where(o => o.BoardID == Id).ToList();
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
                        "delete CMS_R_Board_Pin where  BoardID = \'" + Id + "\'"
                        );

                    /* delete pin */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_Pin where ID not in (SELECT PinID FROM (SELECT PinID FROM CMS_R_KeyWord_Pin UNION SELECT PinID FROM CMS_R_Board_Pin) A)"
                        );

                    /* delete group_board */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_R_GroupBoard_Board where BoardID = \'" + Id + "\'"
                        );

                    /* delete board */
                    _db.Database.ExecuteSqlCommand(
                        "delete CMS_Board where ID = \'" + Id + "\'"
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

        public bool AddBoardToGroup(string boardID, string GroupBoardID, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        /* add new record */
                        var checkExist = _db.CMS_R_GroupBoard_Board.Where(o => o.GroupBoardID == GroupBoardID && o.BoardID == boardID).FirstOrDefault();
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
                            var newGroupBoard = new CMS_R_GroupBoard_Board()
                            {
                                ID = Guid.NewGuid().ToString(),
                                BoardID = boardID,
                                GroupBoardID = GroupBoardID,
                                Status = (byte)Commons.EStatus.Active,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                            };
                            _db.CMS_R_GroupBoard_Board.Add(newGroupBoard);
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

        public bool RemoveKeyFromGroup(string boardID, string groupBoardID, ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        /* add new record */
                        var checkRemove = _db.CMS_R_GroupBoard_Board.Where(o => o.BoardID == boardID && o.GroupBoardID == groupBoardID).FirstOrDefault();
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

        public bool CrawlData(string boardID, string createdBy, ref string msg)
        {
            NSLog.Logger.Info("CrawlData: " + boardID);
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get key by ID */
                    var keyWord = _db.CMS_Board.Where(o => o.ID.Equals(boardID) && o.Status == (byte)Commons.EStatus.Active).FirstOrDefault();
                    if(keyWord != null)
                    {
                        /* check time span crawl */
                        var timeSpanCrawl = DateTime.Now - keyWord.UpdatedDate;
                        /* update crawer date */
                        var bkTime = keyWord.UpdatedDate;
                        keyWord.UpdatedDate = DateTime.Now;
                        keyWord.UpdatedBy = createdBy;
                        _db.SaveChanges();

                        if(!string.IsNullOrEmpty(keyWord.CrawlAccountID))
                        {
                            var _Cookie =  keyWord.CMS_Account.Cookies;
                            if (!string.IsNullOrEmpty(_Cookie))
                                CrawlerBoardHelper._Cookies = _Cookie;
                            else
                                CrawlerHelper._Cookies = "_b = \"AS+B1gn0GdpGgLQl83JubKX1bG19kiuUUvX8lnvITKDHNq2tJcgqXNIQ0cLN+kjq4KM=\"; _pinterest_pfob = enabled; _ga = GA1.2.229901352.1528170174; pnodepath = \"/pin4\"; fba = True; G_ENABLED_IDPS = google; bei = false; logged_out = True; sessionFunnelEventLogged = 1; cm_sub = denied; _auth = 1; csrftoken = fkrSitmDb4vW2kT1G3GfOkcC8mPvl0kV; _pinterest_sess = \"TWc9PSZWaE0xeDZOVm4yL3Yva0VSazRkRjlHR013bk9mdVJBcU9zVEtEOUhXVjhKSFZmZUEreWJiNDYrV3FubVRoVzdqdDF0dmtDcXErcFF6MmlXQUQ3RDVzWERCWTZYZUt4eXMzemkvOGlXRFZQT1J6MjkwampOZlVJUFEvTnNkTUZYMkJ3dGxPTTRKaVIwdGNJY2h1MUhaSHlFT3djd0huNHE0YmtiTTBZR3dVTVB3d0RyYVE4UC8rMjZCYWo2eTJLNGJVSHR6KzRENjlWVE0rNFMxNWdGMUtVL0VtL2RDZktiUFg3M1Y2Z2dEbllPeUxFR3FOdEd6SUJSRTlBMWs1YkJnbTBlWHhwcC9pMmlqRmoydlh0V2VQSGYvYk1zeXlSM2dIU1dmUXIyRWVxWVBPdTYzbHFjcVhYRWRBT0FTQ3VBNmdWMm5QUlREZDdSY2ZQeE1NWklqSUZxNDllVHF1WUVzRFRrRjBXQnZCMVBGTlYxT0UzM1daeHFOUnBBTzliMzFJdmovQ1hQR2Vvc1pkTHNxL1FjT3FrWllTR1d6VHFrd2g5cFBFMmswM3dIa0dOOHVCbGd6aVlKUkJlZlZNeWVyRTBYREcrQVFlUTdRc1NqMlFlQ3RvaWlZMjJXZ1RURmIxNDA2d2JTODRGNk9BYWpoRzVJTUhLMkJ4UDJGb0NmN0NOQXpmZ0FoR08xcElmWmh5S29OeGRadFpDVWR1RGw3ZzZGRS81SlU4UlhSUVlIWm4wRzRJMGFVaTQzdGI3T2ovSCtHR2ZSWlk0M1RCN2JXSmZJRFdQUUpZWVpRMW5ta0pMbXgwT2NZckZJcHg0RTJrTjJlZWJIdXFSdkdJTWNXc2d3NHpXdzFTRGhKVkN4YmY4SCtJaTdSQSt0K2dhc1VDc0tkNnJIeVFhb3BHeDd6OUwvamZsanRKV0ZYNGFmZWFQNGlqNFVqekVFcGUreHU4UGVqZXRuMFVDNE1QbkFuWnJ6YzNjMTF3dVNZUHJ2MjBwMi8xeXNwbnczMlpSa3cvbzVPQUhQSyswNlU4Y2JQaThxNWN1NWtHVm83SWc0YjJVVW1tUWZYcHpWR2RCYS8wRE0yb2RtNUs0NzRteFp4JjVhOXZDbjB5RGtxL1lROE5WOVNDMjB4c1dMND0=\"";
                        }
                        else
                        {
                            CrawlerHelper._Cookies = "_b = \"AS+B1gn0GdpGgLQl83JubKX1bG19kiuUUvX8lnvITKDHNq2tJcgqXNIQ0cLN+kjq4KM=\"; _pinterest_pfob = enabled; _ga = GA1.2.229901352.1528170174; pnodepath = \"/pin4\"; fba = True; G_ENABLED_IDPS = google; bei = false; logged_out = True; sessionFunnelEventLogged = 1; cm_sub = denied; _auth = 1; csrftoken = fkrSitmDb4vW2kT1G3GfOkcC8mPvl0kV; _pinterest_sess = \"TWc9PSZWaE0xeDZOVm4yL3Yva0VSazRkRjlHR013bk9mdVJBcU9zVEtEOUhXVjhKSFZmZUEreWJiNDYrV3FubVRoVzdqdDF0dmtDcXErcFF6MmlXQUQ3RDVzWERCWTZYZUt4eXMzemkvOGlXRFZQT1J6MjkwampOZlVJUFEvTnNkTUZYMkJ3dGxPTTRKaVIwdGNJY2h1MUhaSHlFT3djd0huNHE0YmtiTTBZR3dVTVB3d0RyYVE4UC8rMjZCYWo2eTJLNGJVSHR6KzRENjlWVE0rNFMxNWdGMUtVL0VtL2RDZktiUFg3M1Y2Z2dEbllPeUxFR3FOdEd6SUJSRTlBMWs1YkJnbTBlWHhwcC9pMmlqRmoydlh0V2VQSGYvYk1zeXlSM2dIU1dmUXIyRWVxWVBPdTYzbHFjcVhYRWRBT0FTQ3VBNmdWMm5QUlREZDdSY2ZQeE1NWklqSUZxNDllVHF1WUVzRFRrRjBXQnZCMVBGTlYxT0UzM1daeHFOUnBBTzliMzFJdmovQ1hQR2Vvc1pkTHNxL1FjT3FrWllTR1d6VHFrd2g5cFBFMmswM3dIa0dOOHVCbGd6aVlKUkJlZlZNeWVyRTBYREcrQVFlUTdRc1NqMlFlQ3RvaWlZMjJXZ1RURmIxNDA2d2JTODRGNk9BYWpoRzVJTUhLMkJ4UDJGb0NmN0NOQXpmZ0FoR08xcElmWmh5S29OeGRadFpDVWR1RGw3ZzZGRS81SlU4UlhSUVlIWm4wRzRJMGFVaTQzdGI3T2ovSCtHR2ZSWlk0M1RCN2JXSmZJRFdQUUpZWVpRMW5ta0pMbXgwT2NZckZJcHg0RTJrTjJlZWJIdXFSdkdJTWNXc2d3NHpXdzFTRGhKVkN4YmY4SCtJaTdSQSt0K2dhc1VDc0tkNnJIeVFhb3BHeDd6OUwvamZsanRKV0ZYNGFmZWFQNGlqNFVqekVFcGUreHU4UGVqZXRuMFVDNE1QbkFuWnJ6YzNjMTF3dVNZUHJ2MjBwMi8xeXNwbnczMlpSa3cvbzVPQUhQSyswNlU4Y2JQaThxNWN1NWtHVm83SWc0YjJVVW1tUWZYcHpWR2RCYS8wRE0yb2RtNUs0NzRteFp4JjVhOXZDbjB5RGtxL1lROE5WOVNDMjB4c1dMND0=\"";
                        }
                        
                        var boardUrl = HttpUtility.UrlEncode(keyWord.Url);
                        List<CMS_PinOfBoardModels> models = new List<CMS_PinOfBoardModels>();
                        CrawlerBoardHelper.Get_Tagged_PinOfBoard(ref models, boardUrl,boardID, Commons.PinDefault);
                        if(models != null)
                        {
                            models = models.GroupBy(o => o.id).Select(o => o.First()).ToList();
                            Parallel.ForEach(models, pin =>
                            {
                                CrawlerBoardHelper.Get_Tagged_PinDetail(ref pin, pin.id);
                            });

                            CreateOrUpdatePinOfBoard(models, createdBy, keyWord.ID, ref msg);
                        }
                    }
                }

                NSLog.Logger.Info("ResponseCrawlData: " + boardID, result);
            }
            catch (Exception ex)
            {
                msg = "Crawl data is unsuccessfully.";
                result = false;
                LogHelper.WriteLogs("ErrorCrawlData: " + boardID, JsonConvert.SerializeObject(ex));
                NSLog.Logger.Error("ErrorCrawlData: " + boardID, ex);
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
                    var boards = _db.CMS_Board.Where(o => o.Status == (byte)Commons.EStatus.Active).OrderBy(o => o.CreatedDate).ToList();
                    foreach (var board in boards)
                    {
                        LogHelper.WriteLogs("KeyWords", board.Sequence.ToString());
                        CrawlData(board.ID, createdBy, ref msg);
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
                    var boards = _db.CMS_Board.Where(o => o.Status == (byte)Commons.EStatus.Active).ToList();
                    var boardIDs = boards.Select(o => o.ID).ToList();
                    var GroupBoardDB = _db.CMS_R_GroupBoard_Board.Where(o => boardIDs.Contains(o.BoardID)).ToList();

                    /* delete boards */
                    boards.ForEach(key =>
                    {
                        key.Status = (byte)Commons.EStatus.Deleted;
                        key.UpdatedDate = DateTime.Now;
                        key.UpdatedBy = createdBy;
                    });

                    /* delete group board */
                    GroupBoardDB.ForEach(o =>
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
                    var boards = _db.CMS_Board.Select(o => o.ID).ToList();
                    foreach (var boardID in boards)
                    {
                        DeleteAndRemoveDBCommand(boardID, ref msg);
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


        public bool CreateOrUpdatePinOfBoard(List<CMS_PinOfBoardModels> models,string createdBy,string KeyWordID,  ref string msg)
        {
            var result = true;
            using (var _db = new CMS_Context())
            {
                using (var _trans = _db.Database.BeginTransaction())
                {
                    m_Semaphore.WaitOne();
                    try
                    {
                        _db.Database.CommandTimeout = 500;
                        models = models.GroupBy(x => x.id).Select(x => x.First()).ToList();
                        models = models.Where(x => !string.IsNullOrEmpty(x.id) && x.id.Length <= 60 && x.type.ToLower().Equals("pin")).ToList();
                        var lstPinID = models.Select(o => o.id).ToList();
                        var lstPinUpdate = _db.CMS_Pin.Where(o => lstPinID.Contains(o.ID)).ToList();
                        var lstPinUpdateID = lstPinUpdate.Select(o => o.ID).ToList();
                        var lstPinInsert = models.Where(o => !lstPinUpdateID.Contains(o.id)).ToList();

                        /* update pin */
                        lstPinUpdate = lstPinUpdate.Where(o => o.Status == (byte)Commons.EStatus.Active).ToList();
                        foreach (var uPin in lstPinUpdate)
                        {
                            var repin_Board = models.Where(o => o.id == uPin.ID).Select(o => new { Repin_count = o.repin_count, BoardID = o.board != null ? o.board.id : null, BoardName = o.board != null ? o.board.name : null }).FirstOrDefault();
                            if (repin_Board.Repin_count != uPin.Repin_count)
                            {
                                uPin.Repin_count = repin_Board.Repin_count;
                                uPin.UpdatedBy = createdBy;
                                uPin.UpdatedDate = DateTime.Now;
                                uPin.BoardID = repin_Board.BoardID;
                                uPin.BoardName = repin_Board.BoardName;
                            }
                        }

                        /* insert new pin */
                        var listInsertDB = new List<CMS_Pin>();
                        var listInsertBoard_Pin = new List<CMS_R_Board_Pin>();
                        foreach (var pin in lstPinInsert)
                        {
                            listInsertDB.Add(new CMS_Pin()
                            {
                                ID = pin.id,
                                Link = pin.link,
                                Repin_count = pin.repin_count,
                                ImageUrl = pin.images.Values.Select(o => o.url).First(),
                                Created_At = pin.created_at,
                                Domain = pin.domain,
                                Status = (byte)Commons.EStatus.Active,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = createdBy,
                                UpdatedDate = DateTime.Now,
                                BoardID = pin.board != null ? pin.board.id : null,
                                BoardName = pin.board != null ? pin.board.name : null,
                            });

                            listInsertBoard_Pin.Add(new CMS_R_Board_Pin
                            {
                                ID = Guid.NewGuid().ToString(),
                                BoardID = pin.board != null ? pin.board.id : null,
                                PinID = pin.id,
                                Status = (byte)Commons.EStatus.Active,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = createdBy,
                                UpdatedDate = DateTime.Now,
                            });
                        }
                        if (listInsertDB.Count > 0)
                            _db.CMS_Pin.AddRange(listInsertDB);
                        /* TABLE BOARD_PIN */
                        if (listInsertBoard_Pin.Count > 0)
                            _db.CMS_R_Board_Pin.AddRange(listInsertBoard_Pin);

                        _db.SaveChanges();
                        _trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        NSLog.Logger.Error("CreateOrUpdatePinOfBoard", ex);
                        result = false;
                        _trans.Rollback();
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
    }
}
