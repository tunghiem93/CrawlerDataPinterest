﻿using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSKeyword;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMS_Shared.Keyword
{
    public class KeywordFactory
    {

        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create key, craw data */

        public List<CMS_KeywordModels> GetList()
        {
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get all key word */
                    var listCount = _db.CMS_R_KeyWord_Pin
                        .Join(_db.CMS_KeyWord.Where(o=> o.Status == (byte)Commons.EStatus.Active), kp => kp.KeyWordID, k => k.ID, (kp, k) => new { kp, k })
                        .GroupBy(o => o.k)
                        .Select(o => new CMS_KeywordModels()
                        {
                            Id = o.Key.ID,
                            Quantity = o.Count(),
                        }).ToList();
                    var data = _db.CMS_KeyWord.Where(o => o.Status == (byte)Commons.EStatus.Active).Select(o => new CMS_KeywordModels()
                    {
                        Id = o.ID,
                        Sequence = o.Sequence,
                        KeySearch = o.KeyWord,
                    }).ToList();
                    data.ForEach(o =>
                    {
                        o.Quantity = listCount.Where(c => c.Id == o.Id).Select(c => c.Quantity).FirstOrDefault();
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
                m_Semaphore.WaitOne();
                using (var trans = _db.Database.BeginTransaction())
                {
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
                    var e = _db.CMS_KeyWord.Find(Id);
                    _db.CMS_KeyWord.Remove(e);
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



        public bool CrawlData(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var _db = new CMS_Context())
                {
                    /* get key by ID */
                    var key = _db.CMS_KeyWord.Where(o => o.ID == Id).Select(o => o.KeyWord).FirstOrDefault();
                    if (!string.IsNullOrEmpty(key))
                    {
                        /* call drawler api to crawl data */
                        var modelCrawler = new CMS_CrawlerModels();
                        CrawlerHelper.Get_Tagged_Pins(ref modelCrawler, key, Commons.PinDefault);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Crawl data is unsuccessfully.";
                result = false;
            }
            return result;
        }
    }
}
