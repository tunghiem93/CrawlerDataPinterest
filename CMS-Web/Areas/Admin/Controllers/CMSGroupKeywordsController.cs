using CMS_DTO.CMSGroupKeywords;
using CMS_DTO.CMSKeyword;
using CMS_Shared.CMSGroupKeywords;
using CMS_Shared.Keyword;
using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class CMSGroupKeywordsController : Controller
    {
        // GET: Admin/GroupSearchs
        private CMSGroupKeywordsFactory _factory;
        private CMSKeywordFactory _keyFac;
        private List<string> ListItem = null;
        public CMSGroupKeywordsController()
        {
            _factory = new CMSGroupKeywordsFactory();
            _keyFac = new CMSKeywordFactory();
            ListItem = new List<string>();
            ListItem = _factory.GetList().Select(o => o.Name).ToList();
        }

        // GET: Admin/CMSListKeywords
        public ActionResult Index()
        {
            CMS_GroupKeywordsModels model = new CMS_GroupKeywordsModels();
            return View(model);
        }

        public ActionResult LoadGrid(CMS_GroupKeywordsModels item)
        {
            try
            {
                var msg = "";
                bool isCheck = true;
                if (item.Name != null && item.Name.Length > 0)
                {
                    var temp = ListItem.Where(o => o.Trim() == item.Name.Trim()).FirstOrDefault();
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
                        ViewBag.DuplicateKeyword = "Duplicate Keyword!";
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
            var result = _factory.Delete(ID, "Admin", ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            
            var model = _factory.GetDetail(Id);
            return PartialView("_Edit", model);
        }
    }
}