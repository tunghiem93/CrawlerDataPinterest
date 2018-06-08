using CMS_DTO.CMSSession;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CMS_Web.Areas.Clients.Controllers
{
    public class HQController : Controller
    {
        public HQController()
        {
            var _Path = HostingEnvironment.MapPath("~/Uploads/Silder/");
            var list = Directory.GetFiles(_Path).Select(x => Path.GetFileName(x)).ToList();
            var ListSlider = new List<SliderSession>();
            if (list != null && list.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    ListSlider.Add(new SliderSession
                    {
                        ImageUrl = list[i]
                    });
                }
            }
            System.Web.HttpContext.Current.Session["SliderSession"] = ListSlider;
        }
    }
}