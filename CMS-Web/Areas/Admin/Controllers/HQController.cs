using CMS_DTO.CMSBase;
using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_Shared.CMSBoard;
using CMS_Shared.CMSGroupBoard;
using CMS_Shared.CMSGroupKeywords;
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
                new SelectListItem() {Text="Latest on Pinterest",Value=Commons.ETimeType.TimeReduce.ToString("d") },
                new SelectListItem() {Text="Oldest on Pinterest",Value=Commons.ETimeType.TimeIncrease.ToString("d")},
                new SelectListItem() {Text="Latest on tool",Value=Commons.ETimeType.ToolReduce.ToString("d") },
                new SelectListItem() {Text="Oldest on tool",Value=Commons.ETimeType.ToolIncrease.ToString("d")},
                new SelectListItem() {Text="Repin reduce",Value=Commons.ETimeType.PinReduce.ToString("d") },
                new SelectListItem() {Text="Repin increase",Value=Commons.ETimeType.PinIncrease.ToString("d")},
                //new SelectListItem() {Text="Custom",Value=Commons.ETimeType.TimeCustom.ToString("d")},
            };

            return _lstTime;
        }

        public List<SelectListItem> getListRepinCount()
        {
            var _lstTime = new List<SelectListItem>() {
                new SelectListItem() {Text="Pin reduce",Value=Commons.EPinType.PinReduce.ToString("d") },
                new SelectListItem() {Text="Pin increase",Value=Commons.EPinType.PinIncrease.ToString("d")},
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
            var _facGroup = new CMSGroupKeywordsFactory();
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

        public List<SelectListItem> getListBoard()
        {
            var _fac = new CMSBoardFactory();
            var data = _fac.GetList();
            var lstKeyword = new List<SelectListItem>();
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    lstKeyword.Add(new SelectListItem
                    {
                        Value = item.id,
                        Text = item.name
                    });
                }
            }
            return lstKeyword;
        }

        public List<SelectListItem> getListGroupKeyword()
        {
            var _fac = new CMSGroupKeywordsFactory();
            var data = _fac.GetList();
            var lstGroupKeyword = new List<SelectListItem>();
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    lstGroupKeyword.Add(new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name
                    });
                }
            }
            return lstGroupKeyword;
        }

        public List<string> getListBoardByGroud(string GroupId)
        {
            var _fac = new CMSBoardFactory();
            var data = _fac.GetList(GroupId);
            var lstString = new List<string>();
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    lstString.Add(item.id);
                }
            }
            return lstString;
        }

        public List<string> getListKeyWordByGroup(string GroupId)
        {
            var _fac = new CMSKeywordFactory();
            var data = _fac.GetList(GroupId);
            var lstString = new List<string>();
            if(data != null && data.Any())
            {
                foreach(var item in data)
                {
                    lstString.Add(item.Id);
                }
            }
            return lstString;
        }

        public List<SelectListItem> getListGroupBoards()
        {
            var _fac = new CMSGroupBoardFactory();
            var data = _fac.GetList();
            var lstGroupBoards = new List<SelectListItem>();
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    lstGroupBoards.Add(new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name
                    });
                }
            }
            return lstGroupBoards;
        }
    }
}