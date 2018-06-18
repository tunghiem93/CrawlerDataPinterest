using CMS_DTO.CMSKeyword;
using CMS_Shared.CMSListKeyword;
using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class CMSListKeywordsController : Controller
    {
        private CMSGroupKeywordFactory _factory;

        public CMSListKeywordsController()
        {
            _factory = new CMSGroupKeywordFactory();
        }

        // GET: Admin/CMSListKeywords
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = new CMS_KeywordModels(); // _factory.GetList();
            return PartialView("_ListData", model);
        }

    }
}