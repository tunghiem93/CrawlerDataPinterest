using CMS_DTO.CMSGroupSearch;
using CMS_Shared.GroupSearch;
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
    public class GroupSearchsController : HQController
    {
        // GET: Admin/GroupSearchs
        private GroupSearchFactory _factory;
        public GroupSearchsController()
        {
            _factory = new GroupSearchFactory();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList();
            return PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMS_GroupSearchModels model = new CMS_GroupSearchModels();
            return PartialView("_Create", model);
        }

        //[HttpPost]
        //public ActionResult Create(CMS_GroupSearchModels model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            return PartialView("_Create", model);
        //        }

        //        var msg = "";
        //        var result = _factory.CreateOrUpdate(model, ref msg);
        //        if (result)
        //        {
                    
        //            return RedirectToAction("Index");
        //        }

        //        ModelState.AddModelError("FirstName", msg);
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return PartialView("_Create", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return PartialView("_Create", model);
        //    }
        //}
    }
}