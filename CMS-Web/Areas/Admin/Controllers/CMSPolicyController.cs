using CMS_DTO.CMSPolicy;
using CMS_Shared.CMSPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSPolicyController : Controller
    {
        private CMSPolicyFactory _factory;
        public CMSPolicyController()
        {
            _factory = new CMSPolicyFactory();
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            var model = new CMS_PolicyModels();
            var data = _factory.GetData();
            if (data != null)
                model = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CMS_PolicyModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return View("Index", model);
                }
                var msg = "";
                var result = _factory.InsertOrUpdate(model, ref msg);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("Description", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View("Index", model);
            }
        }
    }
}