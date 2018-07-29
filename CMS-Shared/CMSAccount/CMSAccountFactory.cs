using CMS_DTO.CMSAccount;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSAccount
{
    public class CMSAccountFactory
    {
        public bool CreateOrUpdate(CMS_AccountModels model, ref string msg)
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
                            var checkDup = cxt.CMS_Account.Where(o => o.Account == model.Account).FirstOrDefault();

                            if (checkDup == null)
                            {
                                var curSeq = cxt.CMS_Account.OrderByDescending(o => o.Sequence).Select(o => o.Sequence).FirstOrDefault();

                                var dateTimeNow = DateTime.Now;
                                var newAccount = new CMS_Account()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Account = model.Account,
                                    Password = model.Password,
                                    Status = (byte)Commons.EErrorStatus.AccPending,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = dateTimeNow,
                                    UpdatedBy = model.CreatedBy,
                                    UpdatedDate = dateTimeNow,
                                    Sequence = ++curSeq,
                                };
                                cxt.CMS_Account.Add(newAccount);
                            }
                            else if (checkDup.Status != (byte)Commons.EStatus.Active)
                            {
                                checkDup.Status = (byte)Commons.EErrorStatus.AccPending;
                                checkDup.UpdatedBy = model.CreatedBy;
                                checkDup.UpdatedDate = DateTime.Now;
                            }
                            else /* duplicate key word */
                            {
                                msg = "Duplicate account.";
                            }
                            cxt.SaveChanges();
                            trans.Commit();
                        }
                        else
                        {
                            msg = "Unable to edit account.";
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
                    var e = cxt.CMS_Account.Find(Id);
                    cxt.CMS_Account.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Can't delete this account.";
                result = false;
            }
            return result;
        }

        public List<CMS_AccountModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Account.Select(x => new CMS_AccountModels
                    {
                        Id = x.Id,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate ?? DateTime.Now,
                        IsActive = x.IsActive,
                        Sequence = x.Sequence,
                        Status = x.Status,
                        Account = x.Account,
                        Password = x.Password,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate ?? DateTime.Now,
                    }).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public string GetCookies(string accountID)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Account.Where(w => w.Id.Equals(accountID)).Select(x => x.Cookies).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }
        public bool SaveCookies(string Id, string Cookies, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Account.Find(Id);
                    if (e != null)
                    {
                        e.Cookies = Cookies;
                        e.Status = (byte)Commons.EStatus.Active;
                    }
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Can't update this cookies.";
                result = false;
            }
            return result;
        }

        public bool ChangeStatus(string ID, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Account.Find(ID);
                    if (e != null)
                    {
                        //if (!e.IsActive)
                        //{
                        //    var active = cxt.CMS_Account.Where(o=> o.IsActive).FirstOrDefault();
                        //    if (active != null)
                        //    {
                        //        active.IsActive = false;
                        //    }                            
                        //    e.IsActive = !e.IsActive;
                        //}

                        e.IsActive = !e.IsActive;
                    }

                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Can't update status this account.";
                result = false;
            }
            return result;
        }
    }
}
