using CMS_DTO.CMSCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Clients.Controllers
{
    public class LoginController : Controller
    {
        // GET: Clients/Login
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            var model = new CustomerModels();
            return View(model);
        }
    }
}