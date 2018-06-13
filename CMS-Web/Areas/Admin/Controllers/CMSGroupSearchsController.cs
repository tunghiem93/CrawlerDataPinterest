﻿using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSGroupSearch;
using CMS_Shared;
using CMS_Shared.GroupSearch;
using CMS_Shared.Utilities;
using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class CMSGroupSearchsController : BaseController
    {
        // GET: Admin/GroupSearchs
        private GroupSearchFactory _factory;
        private List<string> ListItem = null;
        public CMSGroupSearchsController()
        {
            _factory = new GroupSearchFactory();
            ListItem = new List<string>();
            ListItem = _factory.GetList().Select(o=>o.KeySearch).ToList();
        }

        public ActionResult Index()
        {
            CMS_GroupSearchModels model = new CMS_GroupSearchModels();
            model.ListKeyResult = _factory.GetList();
            if (model.ListKeyResult != null && model.ListKeyResult.Any())
            {
                int index = 0;
                model.ListKeyResult.ForEach(o =>
                {
                    o.OffSet = index;
                    index++;
                });
            }
            return View(model);
        }

        public ActionResult AddTabKeySearch(int currentOffset, string KeySearch)
        {
            CMS_GroupSearchModels group = new CMS_GroupSearchModels();
            group.CreatedBy = CurrentUser.UserId;
            group.OffSet = currentOffset;
            group.KeySearch = KeySearch;

            var isCheck = ListItem.Where(o => o.Trim() == group.KeySearch.Trim()).FirstOrDefault();
            //Call api get quantity and save database
            if (isCheck == null)
            {
                var msg = "";
                //CrawlerHelper.Get_Tagged_Pins_Count(ref qty, Key, 100);
                var modelCrawler = new CMS_CrawlerModels();
                CrawlerHelper.Get_Tagged_Pins(ref modelCrawler, KeySearch, Commons.PinDefault);
                group.Quantity = modelCrawler.Pins.Count;

                var result = _factory.CreateOrUpdate(group, ref msg);
                if (result)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DeleteItem(string ID)
        {
            CMS_GroupSearchModels group = new CMS_GroupSearchModels();
            var msg = "";
            var result = _factory.Delete(ID, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult RefreshItem(string ID, string Key)
        {
            //Call api get quantity
            var msg = "";
            int qty = 0;
            //CrawlerHelper.Get_Tagged_Pins_Count(ref qty, Key, 100);
            var modelCrawler = new CMS_CrawlerModels();
            CrawlerHelper.Get_Tagged_Pins(ref modelCrawler, Key, Commons.PinDefault);
            qty = modelCrawler.Pins.Count;

            var result = _factory.Refresh(ID, qty, ref msg);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}