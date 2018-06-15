using CMS_DTO.CMSBase;
using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_Shared.Keyword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class HQController : Controller
    {
        // GET: Administration/HQ
        public UserSession CurrentUser
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["User"] != null)
                    return (UserSession)System.Web.HttpContext.Current.Session["User"];
                else
                    return new UserSession();
            }
        }
        public List<SelectListItem> getListTime()
        {
            var _lstTime = new List<SelectListItem>() {
                new SelectListItem() {Text="Thời gian giảm dần",Value=Commons.ETimeType.TimeReduce.ToString("d") },
                new SelectListItem() {Text="Thời gian tăng dần",Value=Commons.ETimeType.TimeIncrease.ToString("d")},
                //new SelectListItem() {Text="Custom",Value=Commons.ETimeType.TimeCustom.ToString("d")},
            };
            
            return _lstTime;
        }

        public List<SelectListItem> getListRepinCount()
        {
            var _lstTime = new List<SelectListItem>() {
                new SelectListItem() {Text="Pin giảm dần",Value=Commons.EPinType.PinReduce.ToString("d") },
                new SelectListItem() {Text="Pin tăng dần",Value=Commons.EPinType.PinIncrease.ToString("d")},
                //new SelectListItem() {Text="Custom",Value=Commons.ETimeType.TimeCustom.ToString("d")},
            };

            return _lstTime;
        }

        public List<SelectListItem> getListQuantity()
        {
            var _lstQuantity = new List<SelectListItem>() {
                new SelectListItem() { Text = "0 - 100", Value = Commons.EQuantityType.ZeroToOne.ToString("d") },
                new SelectListItem() { Text = "100 - 200", Value = Commons.EQuantityType.OneToTwo.ToString("d") },
                new SelectListItem() { Text = "200 - 300", Value = Commons.EQuantityType.TwoToThree.ToString("d") },
                new SelectListItem() { Text = "300 - 400", Value = Commons.EQuantityType.ThreeToFour.ToString("d") },
                new SelectListItem() { Text = "400 - 500", Value = Commons.EQuantityType.FourToFive.ToString("d") },
                new SelectListItem() { Text = "> 500", Value = Commons.EQuantityType.MoreFive.ToString("d") },
            };
            return _lstQuantity;
        }

        public List<SelectListItem> getListKeyword()
        {
            var _fac = new CMSKeywordFactory();
            var data = _fac.GetList();
            var lstKeyword = new List<SelectListItem>();
            if(data != null && data.Any())
            {
                foreach(var item in data)
                {
                    lstKeyword.Add(new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.KeySearch
                    });
                }
            }
            return lstKeyword;
        }
    }
}