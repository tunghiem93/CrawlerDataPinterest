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
    public class CMSGroupSearchsController : HQController
    {
        // GET: Admin/GroupSearchs
        private GroupSearchFactory _factory;
        public CMSGroupSearchsController()
        {
            _factory = new GroupSearchFactory();
        }

        public ActionResult Index()
        {
            CMS_GroupSearchModels model = new CMS_GroupSearchModels();
            return View(model);
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList();
            return PartialView("../Shared/_ListData", model);
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

        public ActionResult AddTabKeySearch(int currentOffset, string KeySearch)
        {
            CMS_GroupSearchModels group = new CMS_GroupSearchModels();
            group.CreatedBy = CurrentUser.UserId;
            group.OffSet = currentOffset;
            group.KeySearch = KeySearch;
            var msg = "";
            var result = _factory.CreateOrUpdate(group, ref msg);
            if (result)
            {
                return PartialView("_TabSearch", group);
            }

            ModelState.AddModelError("FirstName", msg);
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("_TabSearch", group);
        }
    }
}