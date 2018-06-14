using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSImage;
using CMS_DTO.CMSProduct;
using CMS_Shared;
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
    public class CMSProductsController : BaseController
    {
        public CMSProductsController()
        {
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            CMS_ProductsModels model = new CMS_ProductsModels();
            try
            {
                var _Key = Request["Key"]?? "";
                var modelCrawler = new CMS_CrawlerModels();
                modelCrawler.Key = _Key;
                if (!string.IsNullOrEmpty(_Key))
                {
                    CrawlerHelper.Get_Tagged_Pins(ref modelCrawler, _Key, Commons.PinDefault);
                }
                else
                {
                    
                    CrawlerHelper.Get_Tagged_HomePins(ref modelCrawler, Commons.PinDefault);
                    if (Request.Cookies["TypeTime"] != null)
                    {
                        var _TypeTime = Request.Cookies["TypeTime"].Value.ToString();
                        if (_TypeTime.Equals(Commons.ETimeType.TimeReduce.ToString("d")))
                        {
                            modelCrawler.Pins = modelCrawler.Pins.OrderByDescending(x => x.Created_At).ToList();
                            model.TypeTime = Convert.ToInt16(Commons.ETimeType.TimeReduce);
                        }
                        else if (_TypeTime.Equals(Commons.ETimeType.TimeIncrease.ToString("d")))
                        {
                            modelCrawler.Pins = modelCrawler.Pins.OrderBy(x => x.Created_At).ToList();
                            model.TypeTime = Convert.ToInt16(Commons.ETimeType.TimeIncrease);
                        }
                        else
                        {
                            if(Request.Cookies["FromDate"] != null && Request.Cookies["ToDate"] != null)
                            {
                                var _FromDate = Convert.ToDateTime(Request.Cookies["FromDate"].Value);
                                var _ToDate = Convert.ToDateTime(Request.Cookies["ToDate"].Value);
                                model.TypeTime = Convert.ToInt16(Commons.ETimeType.TimeCustom);
                                model.FromDate = _FromDate;
                                model.ToDate = _ToDate;
                                modelCrawler.Pins = modelCrawler.Pins.Where(x => x.Created_At >= _FromDate && x.Created_At <= _ToDate).ToList();
                            }
                           
                        }
                    }
                }
                model.ListTime = getListTime();
                model.ListQuantity = getListQuantity();
                model.Crawler = modelCrawler;
                model.Crawler.Pins.ForEach(x =>
                {
                    var _Now = DateTime.Now;
                    if (_Now == x.CreatedDate)
                    {
                        var time = _Now.Hour - x.CreatedDate.Hour;
                        if (time >= 1)
                            x.LastTime = "Khoảng " + time + " giờ trước";
                        else
                            x.LastTime = "Khoảng " +( _Now.Minute - x.CreatedDate.Minute) + "phút trước";
                    }
                    else if (_Now > x.CreatedDate)
                    {
                        var time = _Now.Year - x.CreatedDate.Year;
                        if (time > 0)
                            x.LastTime = "Khoảng " + time + " năm trước";
                        else
                        {
                            time = _Now.Month - x.CreatedDate.Month;
                            if (time > 0)
                                x.LastTime = "Khoảng " + time + " tháng trước";
                            else
                            {
                                time = _Now.Day - x.CreatedDate.Day;
                                if (time > 0)
                                    x.LastTime = "Khoảng " + time + " ngày trước";
                                else
                                {
                                    time = _Now.Hour - x.CreatedDate.Hour;
                                    if (time > 0)
                                    {
                                        x.LastTime = "Khoảng " + time + "Khoảng giờ trước";
                                    }
                                    else
                                    {
                                        x.LastTime = "Khoảng "+ (_Now.Minute - x.CreatedDate.Minute) + " phút trước";
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch(Exception ex)
            {

            }
            return View(model);
        }

        public ActionResult LoadGrid()
        {
            //var model = _factory.GetList();
            return null; // PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMS_ProductsModels model = new CMS_ProductsModels();
            return PartialView("_Create", model);
        }

        public CMS_ProductsModels GetDetail(string Id)
        {
            return null; // _factory.GetDetail(Id);
        }

        [HttpPost]
        public ActionResult Create(CMS_ProductsModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }
                byte[] photoByte = null;
                Dictionary<int, byte[]> lstImgByte = new Dictionary<int, byte[]>();
                var data = new List<CMS_ImagesModels>();
                if (model.PictureUpload.Length > 0 && model.PictureUpload.Any() && model.PictureUpload[0] != null)
                {
                    
                }
                
                var msg = "";
                var result = true; // _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    foreach (var item in data)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Products/" + item.TempImageURL)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Products/" + item.TempImageURL));
                            }

                            var path = Server.MapPath("~/Uploads/Products/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte,400,Commons.WidthProduct,Commons.HeightProduct);
                            model.PictureByte = photoByte;
                        }
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("ProductCode", msg);
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
            var _OffSet = 0;
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMS_ProductsModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                byte[] photoByte = null;
                Dictionary<int, byte[]> lstImgByte = new Dictionary<int, byte[]>();
                var data = new List<CMS_ImagesModels>();
                
                var msg = "";
                var result = true; // _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    foreach (var item in data)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Products/" + item.TempImageURL)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Products/" + item.TempImageURL));
                            }

                            var path = Server.MapPath("~/Uploads/Products/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte, 400, Commons.WidthProduct, Commons.HeightProduct);
                            model.PictureByte = photoByte;
                        }
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("ProductCode", msg);
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
        public ActionResult Delete(CMS_ProductsModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Delete", model);
                }
                var msg = "";
                var _LstImageOfProduct = true; // _factory.GetListImageOfProduct(model.Id);
                var result = true; // _factory.Delete(model.Id, ref msg);
                //if (result)
                //{
                //    if(_LstImageOfProduct != null && _LstImageOfProduct.Any())
                //    {
                //        foreach(var item in _LstImageOfProduct)
                //        {
                //            // delete image for folder
                //            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Products/" + item.ImageURL)))
                //            {
                //                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Products/" + item.ImageURL));
                //            }
                //        }
                //    }
                //    return RedirectToAction("Index");
                //}
                    
                //ModelState.AddModelError("ProductCode", msg);
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
        }

        [HttpPost]
        public PartialViewResult AddImageItem(int OffSet, int Length)
        {

            List<CMS_ImagesModels> model = new List<CMS_ImagesModels>();
            var _OffSet = OffSet;
            for (int i = 0; i < Length; i++)
            {
                model.Add(new CMS_ImagesModels
                {
                    OffSet = _OffSet,
                    IsDeleted = false
                });
                _OffSet = _OffSet + 1;
            }
            return PartialView("_ListItem", model);
        }

        [HttpPost]
        public ActionResult DeleteImage(string Id,string ProductId)
        {
            try
            {
                string msg = "";
                var result = true; // _factory.DeleteImage(Id,ref msg);
                if (!result)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new HttpStatusCodeResult(400, "Have an error when you delete a Payment Method");
                }
                // delete image for folder
                if (System.IO.File.Exists(Server.MapPath("~/Uploads/Products/" + msg)))
                {
                    ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Products/" + msg));
                }
                var model = new CMS_ProductsModels();
               
                var _OffSet = 0;
                
                return PartialView("_ListItem", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult ProductDetail(string id,string Key)
        {
            var modelCrawler = new CMS_CrawlerModels();
            try
            {
                var model = new PinsModels();
                CrawlerHelper.Get_Tagged_PinsDetail(ref model, id);
                CrawlerHelper.Get_Tagged_OrtherPins(ref modelCrawler, Key, Commons.PinOrtherDefault, "", 1, id);
                modelCrawler.Pin = model;
            }
            catch (Exception) { }
            return View(modelCrawler);
        }

        public ActionResult Search()
        {
            try
            {
                var _Key = Request["Key"] ?? "";
                var TypeTime = Request["TypeTime"] ?? "";
                //cache data
                if(string.IsNullOrEmpty(_Key))
                {
                    Response.Cookies["TypeTime"].Value = TypeTime;
                    Response.Cookies["TypeTime"].Expires = DateTime.Now.AddYears(1); // add expiry time
                }
                var modelCrawler = new CMS_CrawlerModels();
                modelCrawler.Key = _Key;
                if (!string.IsNullOrEmpty(_Key))
                {
                    CrawlerHelper.Get_Tagged_Pins(ref modelCrawler, _Key, Commons.PinDefault);
                }
                else
                {
                    CrawlerHelper.Get_Tagged_HomePins(ref modelCrawler, Commons.PinDefault);
                }
                if(TypeTime.Equals(Commons.ETimeType.TimeReduce.ToString("d")))
                {
                    modelCrawler.Pins = modelCrawler.Pins.OrderByDescending(x => x.Created_At).ToList();
                }
                else if(TypeTime.Equals(Commons.ETimeType.TimeIncrease.ToString("d")))
                {
                    modelCrawler.Pins = modelCrawler.Pins.OrderBy(x => x.Created_At).ToList();
                }
                else
                {
                    var _FromDate = Convert.ToDateTime(Request["FromDate"]);
                    var _ToDate = Convert.ToDateTime(Request["ToDate"]);
                    Response.Cookies["FromDate"].Value = _FromDate.ToString();
                    Response.Cookies["FromDate"].Expires = DateTime.Now.AddYears(1); // add expiry time
                    Response.Cookies["ToDate"].Value = _ToDate.ToString();
                    Response.Cookies["ToDate"].Expires = DateTime.Now.AddYears(1); // add expiry time
                    modelCrawler.Pins = modelCrawler.Pins.Where(x => x.Created_At >= _FromDate && x.Created_At <= _ToDate).ToList();
                }
                return PartialView("_ListItem", modelCrawler);
            }
            catch (Exception) { }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}