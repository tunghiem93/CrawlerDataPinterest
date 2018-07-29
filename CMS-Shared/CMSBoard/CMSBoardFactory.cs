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
    }
}
