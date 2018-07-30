using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSProduct;
using CMS_Shared;
using CMS_Shared.CMSBoard;
using CMS_Shared.CMSEmployees;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSBoardProductsController : BaseController
    {
        private CMSPinFactory _fac;
        private CMSBoardFactory _facBoard;
        public CMSBoardProductsController()
        {
            _fac = new CMSPinFactory();
            _facBoard = new CMSBoardFactory();
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            CMS_ProductsModels model = new CMS_ProductsModels();
            try
            {
                var _Key = Request["BoardID"] ?? "";
                var _Group = Request["GroupID"] ?? "";
                var FilterModel = new PinFilterDTO();
                FilterModel.PageIndex = Commons.PageIndex;
                FilterModel.PageSize = Commons.PageSize;
                FilterModel.TypeTime = Commons.ETimeType.TimeReduce.ToString("d");
                model.ListTime = getListTime();
                model.ListQuantity = getListQuantity();
                // model.ListRePin = getListRepinCount();
                var lstBoard = getListBoard();
                ViewBag.Keywords = lstBoard;
                if (!string.IsNullOrEmpty(_Key))
                {
                    FilterModel.LstBoardID.Add(_Key);
                    model.listBoards.Add(_Key);
                }
                if (!string.IsNullOrEmpty(_Group))
                {
                    FilterModel.LstGroupBoardID.Add(_Group);

                    var _lstKeywords = getListBoardByGroud(_Group);
                    FilterModel.LstBoardID = _lstKeywords;
                    model.listBoards.AddRange(_lstKeywords);
                }

                if(FilterModel.LstBoardID.Count == 0)
                {
                    FilterModel.LstBoardID = lstBoard.Select(o => o.Value).ToList();
                }

                var _pinModels = new List<PinsModels>();
                var msg = "";
                int totalPin = 0;
                var result = _fac.GetPin(ref _pinModels, ref totalPin, FilterModel, ref msg);
                if (result)
                {
                    model.Crawler.Pins = _pinModels.OrderBy(x => x.Created_At).ToList();
                    #region "Comment"
                    //if (TypeTime.Equals(Commons.ETimeType.TimeReduce.ToString("d")))
                    //{
                    //    model.Crawler.Pins = model.Crawler.Pins.OrderByDescending(x => x.Created_At).ToList();
                    //}
                    //else if (TypeTime.Equals(Commons.ETimeType.TimeIncrease.ToString("d")))
                    //{
                    //    model.Crawler.Pins = model.Crawler.Pins.OrderBy(x => x.Created_At).ToList();
                    //}
                    //else if (TypePin.Equals(Commons.ETimeType.PinReduce.ToString("d")))
                    //{
                    //    model.Crawler.Pins = model.Crawler.Pins.OrderByDescending(x => x.Repin_count).ToList();
                    //}
                    //else if (TypePin.Equals(Commons.ETimeType.PinIncrease.ToString("d")))
                    //{
                    //    model.Crawler.Pins = model.Crawler.Pins.OrderBy(x => x.Repin_count).ToList();
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        public ActionResult LoadScroll(PinFilterDTO pinFilter)
        {
            try
            {
                if (!string.IsNullOrEmpty(pinFilter.Url))
                {
                    NameValueCollection QueryString = CommonHelper.GetQueryParameters(pinFilter.Url);
                    var _Key = QueryString["BoardID"] ?? "";
                    var _Group = QueryString["GroupID"] ?? "";

                    if (!string.IsNullOrEmpty(_Key))
                    {
                        pinFilter.LstBoardID.Add(_Key);
                    }
                    if (!string.IsNullOrEmpty(_Group))
                    {
                        pinFilter.LstGroupBoardID.Add(_Group);

                        var _lstKeywords = getListBoardByGroud(_Group);
                        pinFilter.LstBoardID.AddRange(_lstKeywords);
                    }
                }

                if (pinFilter.LstBoardID != null && pinFilter.LstBoardID.Count > 0)
                {
                    if (string.IsNullOrEmpty(pinFilter.LstBoardID[0]))
                        pinFilter.LstBoardID = null;
                }

                if (pinFilter.LstBoardID == null || pinFilter.LstBoardID.Count == 0)
                    pinFilter.LstBoardID = getListBoard().Select(o => o.Value).ToList();

                var modelCrawler = new CMS_CrawlerModels();
                var _pinModels = new List<PinsModels>();
                var msg = "";
                pinFilter.PageSize = Commons.PageSize;
                int totalPin = 0;
                var result = _fac.GetPin(ref _pinModels, ref totalPin, pinFilter, ref msg);
                if (result)
                {
                    modelCrawler.Pins = _pinModels;
                }
                return PartialView("_ListItem", modelCrawler);
            }
            catch (Exception) { }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Search()
        {
            try
            {
                var FilterModel = new PinFilterDTO();
                FilterModel.PageIndex = Commons.PageIndex;
                FilterModel.PageSize = Commons.PageSize;
                FilterModel.CreatedDateFrom = null;
                FilterModel.CreatedDateTo = null;
                // var _Key = Request["Key"] ?? "";
                var TypeTime = Request["TypeTime"] ?? "";
                //  var TypePin = Request["TypePin"] ?? "";
                var _TypeQuantity = Request["TypeQuantity"];
                int TypeQuantity = -1;
                if (!string.IsNullOrEmpty(_TypeQuantity))
                {
                    TypeQuantity = Convert.ToInt16(_TypeQuantity);
                }
                var KeyBoard = Request["listBoards"] ?? null;
                char[] separator = new char[] { ',' };
                var ListBoards = CommonHelper.ParseStringToList(KeyBoard, separator);
                var _FromDate = Convert.ToDateTime(Request["FromDate"]);
                var _ToDate = Convert.ToDateTime(Request["ToDate"]);
                #region "comment"
                //cache data
                //Response.Cookies["TypeTime"].Value = TypeTime.ToString();
                //Response.Cookies["TypeTime"].Expires = DateTime.Now.AddYears(1); // add expiry time

                //Response.Cookies["TypePin"].Value = TypePin.ToString();
                //Response.Cookies["TypePin"].Expires = DateTime.Now.AddYears(1); // add expiry time

                //Response.Cookies["FromDate"].Value = _FromDate.ToString();
                //Response.Cookies["FromDate"].Expires = DateTime.Now.AddYears(1); // add expiry time
                //Response.Cookies["ToDate"].Value = _ToDate.ToString();
                //Response.Cookies["ToDate"].Expires = DateTime.Now.AddYears(1); // add expiry time
                //if(TypeQuantity != 0)
                //{
                //    Response.Cookies["TypeQuantity"].Value = TypeQuantity.ToString();
                //    Response.Cookies["TypeQuantity"].Expires = DateTime.Now.AddYears(1); // add expiry time
                //}
                #endregion
                FilterModel.CreatedAtFrom = _FromDate;
                FilterModel.CreatedAtTo = _ToDate;
                if (TypeQuantity.ToString() == Commons.EQuantityType.ZeroToOne.ToString("d"))
                {
                    FilterModel.PinCountFrom = 0;
                    FilterModel.PinCountTo = 100;
                }
                if (TypeQuantity.ToString() == Commons.EQuantityType.OneToTwo.ToString("d"))
                {
                    FilterModel.PinCountFrom = 100;
                    FilterModel.PinCountTo = 200;
                }
                if (TypeQuantity.ToString() == Commons.EQuantityType.TwoToThree.ToString("d"))
                {
                    FilterModel.PinCountFrom = 200;
                    FilterModel.PinCountTo = 300;
                }
                if (TypeQuantity.ToString() == Commons.EQuantityType.ThreeToFour.ToString("d"))
                {
                    FilterModel.PinCountFrom = 300;
                    FilterModel.PinCountTo = 400;
                }
                if (TypeQuantity.ToString() == Commons.EQuantityType.FourToFive.ToString("d"))
                {
                    FilterModel.PinCountFrom = 400;
                    FilterModel.PinCountTo = 500;
                }
                if (TypeQuantity.ToString() == Commons.EQuantityType.MoreFive.ToString("d"))
                {
                    FilterModel.PinCountFrom = 500;
                }

                if (ListBoards != null && ListBoards.Count > 0)
                {
                    FilterModel.LstBoardID = ListBoards;
                    // Response.Cookies["Keywords"].Value = Keywords.ToString();
                    //  Response.Cookies["Keywords"].Expires = DateTime.Now.AddYears(1); // add expiry time
                }
                else
                {
                    FilterModel.LstBoardID = getListBoard().Select(o => o.Value).ToList();
                }
                FilterModel.TypeTime = TypeTime;

                var modelCrawler = new CMS_CrawlerModels();
                var _pinModels = new List<PinsModels>();
                var msg = "";
                int totalPin = 0;
                var result = _fac.GetPin(ref _pinModels, ref totalPin, FilterModel, ref msg);
                if (result)
                {
                    modelCrawler.Pins = _pinModels;

                }
                return PartialView("_ListItem", modelCrawler);
            }
            catch (Exception ex) { }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DeletePin(string ID)
        {
            var msg = "";
            var result = _fac.HidePin(ID, "Admin", ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }    
}