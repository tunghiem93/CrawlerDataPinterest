using CMS_DTO.CMSBoard;
using CMS_Shared.CMSBoards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSBoardsController : HQController
    {
        private CMSBoardsFactory _factory;
        public CMSBoardsController()
        {
            _factory = new CMSBoardsFactory();
            ViewBag.Category = GetListCategorySelectItem();
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList();
            model.ForEach(x =>
            {
                x.sStatus = x.IsActive ? "Kích hoạt" : "Chưa kích hoạt";
            });
            return PartialView("_ListData",model);
        }

        public ActionResult Create()
        {
            CMSBoardModels model = new CMSBoardModels();
            return PartialView("_Create", model);
        }

        public CMSBoardModels GetDetail(string Id)
        {
            return _factory.GetDetail(Id);
        }

        [HttpPost]
        public ActionResult Create(CMSBoardModels model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model,ref Id,ref msg);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("CategoryCode", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
            catch(Exception ex) {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMSBoardModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref Id, ref msg);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("CategoryCode", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Edit", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Edit", model);
            }
        }

        [HttpGet]
        public ActionResult View(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMSBoardModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Delete", model);
                }
                var msg = "";
                var result = _factory.Delete(model.Id, ref msg);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("CategoryName", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
        }
    }
}