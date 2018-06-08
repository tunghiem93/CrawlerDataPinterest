using CMS_DTO.CMSNews;
using CMS_Shared;
using CMS_Shared.CMSNews;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSNewsController : Controller
    {
        private CMSNewsFactory _factory;
        public CMSNewsController()
        {
            _factory = new CMSNewsFactory();
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
                if (!string.IsNullOrEmpty(x.ImageURL))
                    x.ImageURL = Commons.HostImage + "News/" + x.ImageURL;
            });
            return PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMS_NewsModels model = new CMS_NewsModels();
            return PartialView("_Create", model);
        }

        public CMS_NewsModels GetDetail(string Id)
        {
            var model = _factory.GetDetail(Id);
            //if (model != null)
            //    model.ImageURL = Commons.HostImage + "News/" + model.ImageURL;
            return model;
        }

        [HttpPost]
        public ActionResult Create(CMS_NewsModels model)
        {
            try
            {
                byte[] photoByte = null;
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }
                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = imgByte;
                    model.ImageURL = Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName);
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        var path = Server.MapPath("~/Uploads/News/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte,400,Commons.WidthImageNews,Commons.HeightImageNews);
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("Title", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var model = GetDetail(Id);
            if (!string.IsNullOrEmpty(model.ImageURL))
                model.ImageURL = Commons.HostImage +"News/" + model.ImageURL;
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMS_NewsModels model)
        {
            var temp = model.ImageURL;
            try
            {
                byte[] photoByte = null;
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages +"News/", "").Replace(Commons.Image272_259, "");
                }

                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = imgByte;
                    model.ImageURL = Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName);
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        if (!string.IsNullOrEmpty(temp))
                        {
                            temp = temp.Replace(Commons._PublicImages + "News/", "").Replace(Commons.Image272_259, "");
                            temp = "~/Uploads/News/" + temp;
                            if (System.IO.File.Exists(Server.MapPath(temp)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath(temp));
                            }
                        }
                        var path = Server.MapPath("~/Uploads/News/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 400, Commons.WidthImageNews, Commons.HeightImageNews);
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("Title", msg);
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
            if (!string.IsNullOrEmpty(model.ImageURL))
                model.ImageURL = Commons.HostImage + "News/" + model.ImageURL;
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMS_NewsModels model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    return PartialView("_Delete", model);
                //}
                var msg = "";
                var result = _factory.Delete(model.Id, ref msg);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("Title", msg);
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