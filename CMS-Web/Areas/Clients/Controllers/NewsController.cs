using CMS_DTO.CMSNews;
using CMS_Shared;
using CMS_Shared.CMSNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Clients.Controllers
{
    public class NewsController : Controller
    {
        private CMSNewsFactory _fac;
        public NewsController()
        {
            _fac = new CMSNewsFactory();
        }
        // GET: Clients/News
        public ActionResult Index()
        {
            var model = new CMS_NewsViewModel();
            var data = _fac.GetList().OrderByDescending(x=>x.CreatedDate).ToList();
            if(data != null)
            {
                data.ForEach(x =>
                {
                    x.ImageURL = Commons.HostImage + "News/" + x.ImageURL;
                });

                model.ListNews = data;
                model.ListNewsNew = data.OrderByDescending(x => x.CreatedDate).Skip(0).Take(5).ToList();
            }
            return View(model);
        }

        public ActionResult NewsDetail(string id)
        {
            var model = new CMS_NewsViewModel();
            try
            {
                if(string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index", "NotFound", new { area = "Clients" });
                }
                else
                {
                    var data = _fac.GetDetail(id);
                    if(data != null)
                    {
                        if(!string.IsNullOrEmpty(data.ImageURL))
                        {
                            data.ImageURL = Commons.HostImage + "News/" + data.ImageURL;
                        }
                    }
                    model.CMS_News = data;
                    model.ListNewsNew = _fac.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(5).ToList();
                    if(model.ListNewsNew != null && model.ListNewsNew.Any())
                    {
                        model.ListNewsNew.ForEach(x =>
                        {
                            if (!string.IsNullOrEmpty(x.ImageURL))
                            {
                                x.ImageURL = Commons.HostImage + "News/" + x.ImageURL;
                            }
                            else
                            {
                                x.ImageURL = Commons.Image272_259;
                            }
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "NotFound", new { area = "Clients" });
            }
            return View(model);
        }
    }
}