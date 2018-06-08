using CMS_Shared.CMSPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Clients.Controllers
{
    public class PolicyController : Controller
    {
        // GET: Clients/Policy
        private CMSPolicyFactory _fac;
        public PolicyController()
        {
            _fac = new CMSPolicyFactory();
        }
        public ActionResult Index()
        {
            var model = _fac.GetData();
            if (model == null)
                model = new CMS_DTO.CMSPolicy.CMS_PolicyModels();
            return View(model);
        }
    }
}