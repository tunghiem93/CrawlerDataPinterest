using CMS_DTO.CMSAccount;
using CMS_Shared.CMSAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSPIAccountsController : HQController
    {
        private CMSAccountFactory _factory;
        private CMSAccountFactory _facFBAccount;
        private List<string> ListItem = null;
        public CMSPIAccountsController()
        {
            _factory = new CMSAccountFactory();
            _facFBAccount = new CMSAccountFactory();
            ListItem = new List<string>();
            //ListItem = _factory.GetList().Select(o => o.Account).ToList();
            var lstAcc = _factory.GetList();
            if (lstAcc != null)
                ListItem = lstAcc.Select(o => o.Account).ToList();
        }

        // GET: Admin/CMSFBAccounts
        public ActionResult Index()
        {
            CMS_AccountModels model = new CMS_AccountModels();
            return View(model);
        }
        public ActionResult LoadGrid(CMS_AccountModels item)
        {
            try
            {
                var msg = "";
                bool isCheck = true;
                if (item.Account != null && item.Account.Length > 0 && item.Cookies != null && item.Cookies.Length > 0)
                {
                    var temp = ListItem.Where(o => o.Trim() == item.Account.Trim()).FirstOrDefault();
                    if (temp == null)
                    {
                        var result = _factory.CreateOrUpdate(item, ref msg);
                        if (!result)
                        {
                            isCheck = false;
                        }
                    }
                    else
                    {
                        ViewBag.DuplicateKeyword = "Duplicate account!";
                    }
                }
                if (isCheck)
                {
                    var model = _factory.GetList();
                    return PartialView("_ListData", model);
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                //_logger.Error("Keyword_Search: " + e);
                return new HttpStatusCodeResult(400, e.Message);
            }
        }

        public ActionResult Delete(string ID)
        {
            var msg = "";
            var result = _factory.Delete(ID, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetCookiesPIAccount(string AccountID)
        {
            string msg = "";
            var cookies = _facFBAccount.GetCookies(AccountID);
            if (cookies != null)
            {
                string str = "{'Cookies':'" + cookies + "'}";
                JavaScriptSerializer jsSer = new JavaScriptSerializer();
                object obj = jsSer.Deserialize(str, typeof(object));
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
                return new HttpStatusCodeResult(400, msg);
        }

        public ActionResult SaveCookiesPIAccount(string ID, string Cookies)
        {
            var msg = "";
            var result = _factory.SaveCookies(ID, Cookies, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ChangeStatus(string AccountID)
        {
            var msg = "";
            var result = _factory.ChangeStatus(AccountID, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ChangAccDefault(string AccountID)
        {
            var msg = "";
            var result = _factory.ChangAccDefault(AccountID, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}