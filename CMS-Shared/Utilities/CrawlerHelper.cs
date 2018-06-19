using CMS_DTO.CMSCrawler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace CMS_Shared.Utilities
{
    public static class CrawlerHelper
    {
        public static bool Get_Tagged_Pins(ref CMS_CrawlerModels model, string search_str, int limit = 1, string bookmarks_str = null, int page = 1)
        {
            if (page > limit) return false;
            var next_page = false;
            if (!string.IsNullOrEmpty(bookmarks_str))
                next_page = true;

            string data = string.Empty;
            var urlOrg = Commons.HostApi + search_str;
            var path = string.Empty;
            if (!next_page)
            {
                var objJson = new
                {
                    options = new
                    {
                        scope = "pins",
                        show_scope_selector = true,
                        query = search_str
                    },
                    context = new
                    {
                        app_version = "aad9791"
                    },
                    module = new
                    {
                        name = "SearchPage",
                        options = new
                        {
                            scope = "pins",
                            query = search_str
                        }
                    },
                    append = false,
                    error_strategy = 0
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApi + search_str + "&rs=typed&term_meta[]= " + search_str + "|typed";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }
            else
            {
                var objJson = new
                {
                    options = new
                    {
                        scope = "pins",
                        show_scope_selector = "null",
                        query = search_str,
                        bookmarks = new string[] { bookmarks_str },
                    },
                    context = new
                    {
                        app_version = "2f83a7e"
                    },
                    module = new
                    {
                        name = "GridItems",
                        options = new
                        {
                            scope = "pins",
                            scrollable = true,
                            show_grid_footer = true,
                            centered = true,
                            reflow_all = true,
                            virtualize = true,
                            item_options = new
                            {
                                show_pinner = true,
                                show_pinned_from = false,
                                show_board = true
                            },
                            layout = "variable_height",
                        }
                    },
                    append = true,
                    error_strategy = 2
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApi + search_str + "&rs=typed&term_meta[]=" + search_str + "|typed";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }

            // data = HttpContext.Current.Server.UrlEncode(data);
            var timestamp = GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "" + path + "&_=" + timestamp;
            var bookmarks = "";
            getDataPinterest(url, model, "", ref bookmarks);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_Pins(ref model, search_str, limit, bookmarks, ++page);
            }

            return false;
        }

        public static CMS_CrawlerModels getDataPinterest(string url, CMS_CrawlerModels model, string pinId, ref string bookmarks)
        {

            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = "_auth=1; csrftoken=dMWi2a6L1DTFUHmyqem0oGrDmteiaETw; _pinterest_sess=\"TWc9PSZsSlA1dUF4QWlRWGRYVGR6Qm9mN3pwczUyUDk4ZDYvckduSjl4N3ZSRHlsU1VmWkhBTUsrMU9KNkxjS3pyUk1zREdDL2Rmb2VuT1dwRDhSTmxTOE1Ja0FjOUtreTJVc0o0SmthQ2xhN3lRa3BQVnRMcUF5dlN0Z255Syt4am56VnQvYVQwT0JyejBCSlk4YzFyQ0pEekZwNSs0YjZnMTBseEIvRkU0Um1XeWthZ1cvNGxpdDVyTEdrSHRzWFVLN244T25TaGVoYy93TGVSRjVxNzl5dnlZV1A5L3NlNnc5MWE4djl0ZjNoeEhqTTNuaGduRnZ2VkF1RTd6V1V3VnBCT3cyMksxMHJIdVE0TVVjc3FmWVozVllzekhpNFRGNDFBTERIVzdkcUNUS3NlWEJFdE1mSXJBbnNPVStHQXJiUWJRSENyVVVKTVJYNit5MkZTMFVNN3ptY09FNmFoaHk3Nk9MdUtuRmdDSWRWRVhPTWYrSXA4dFhlRU1hYW5paFNQMU5OcFNwY2xSZlJHZVlWWU03eHFsNWVmSWRHL0ZtN3NhdU9ubzhpUjZqMzNTTUxwMTlOQWRGa29zVUc1UXFqZ1BUYzhHL3M0YndDY2ZBN2ZMZnJQZTlGbXdPWjg5SXJVOEpUMEtPVnMzcjZPcytOVHRFUnlRUnoyNmJZdjl0YXJlOVp1WGQvM29SSi9xWUwvYmFPcDl5VFl1aEw2ZFBtMHlhZ0g4MXlIMXp1dnFXWWY1VytmY0ZPc0FSMzhqYXdhNTBqQjlYRHJ6OE9CY1ViMmljZkFhQkVydGxyVUtlNis4cnh3R3NPbXVTVjZCZUNTR1NKQ3JpWFJsajBsSEFGcytOMnptN2R2S1BXN1NocTFtZVlKMzF0Y1hyQXNseG9DdzdrQklxNnZXMkk2dXQ4azJJOTR4YWlIUDMvVzAwcmQ0SDVqNnhYc3NlTTNpK0ZHUU9xaUpCOER0N1pQaWFFTUhLRGxpdk1EVDlOYi9DdmRLcTQvdUROekpjRXNJSjVtcEl1bWVLUHhRdTVQQk91L1RWS0w0YkkzZDNwaW5mRnJFakRsck9aNTRBUXVsVFdFWVlTRHJ5OUxBWHdMa0V4Jk1FSHZIUWlQUlE2Q05OZWJydEZrV25SQ2tmND0 = \"; G_ENABLED_IDPS=google; _b=\"ATWTNNfXaINNj5j6VvA6 + rquchpAz7VF + IS8VabE7fJo7ragqOV82ASwCOgxcnxHC5k = \"; pnodepath=\" / 4\"; _ga=GA1.2.1908176321.1528170001; fba=True; cm_sub=none; sessionFunnelEventLogged=1; bei=false";
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var results = (dynamic)null;
                                if (string.IsNullOrEmpty(pinId))
                                {
                                    results = data["results"];
                                }
                                else
                                {
                                    results = data;
                                }

                                if (results != null)
                                {
                                    foreach (var item in results)
                                    {
                                        var pin = new PinsModels();
                                        var itemPin = item as Dictionary<string, dynamic>;
                                        bool flag = true;
                                        pin.CreatedDate = DateTime.Now;
                                        if (itemPin.ContainsKey("domain"))
                                        {
                                            pin.Domain = itemPin["domain"];
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("id"))
                                        {
                                            pin.ID = itemPin["id"];

                                            /* check exist pin */
                                            var checkExist = model.Pins.Where(o => o.ID == pin.ID).FirstOrDefault();
                                            if (checkExist == null) /* new pin */
                                            {
                                                // get Repin_Count
                                                var _Repin_Count = 0;
                                                Get_Tagged_RepinCount(ref _Repin_Count, pin.ID);
                                                pin.Repin_count = _Repin_Count;
                                            }
                                            else /* this pin have exist in model */
                                            {
                                                flag = false;
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("link"))
                                        {
                                            pin.Link = itemPin["link"];
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("created_at"))
                                        {
                                            pin.Created_At = DateTime.Parse(itemPin["created_at"], new CultureInfo("en-US", true));
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("images"))
                                        {
                                            var Images = itemPin["images"] as Dictionary<string, dynamic>;
                                            if (Images != null)
                                            {
                                                foreach (var itemImg in Images)
                                                {
                                                    var Image = itemImg.Value;
                                                    var _ImageModel = new ImageModels()
                                                    {
                                                        url = Image["url"],
                                                        height = Convert.ToInt16(Image["height"]),
                                                        width = Convert.ToInt16(Image["width"])
                                                    };
                                                    pin.Images.Add(_ImageModel);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (flag)
                                        {
                                            model.Pins.Add(pin);
                                        }

                                    }
                                }
                            }

                            var dataBookmark = resource_data_cache[0]["response"];
                            if (dataBookmark != null)
                            {
                                bookmarks = dataBookmark["bookmark"];
                            }
                        }
                    }

                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorgetDataPinterest" + "\n url: " + url + "\nBookmarks:" + bookmarks, ex);
            }
            return model;
        }

        public static string Preg_replace(this string input, string[] pattern, string[] replacements)
        {
            if (replacements.Length != pattern.Length)
                throw new ArgumentException("Replacement and Pattern Arrays must be balanced");

            for (var i = 0; i < pattern.Length; i++)
            {
                input = Regex.Replace(input, pattern[i], replacements[i]);
            }

            return input;
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }

        public static bool Get_Tagged_OrtherPins(ref CMS_CrawlerModels model, string search_str, int limit = 1, string bookmarks_str = null, int page = 1, string pinId = "")
        {
            if (page > limit) return false;
            var next_page = false;
            if (!string.IsNullOrEmpty(bookmarks_str))
                next_page = true;

            string data = string.Empty;
            var urlOrg = Commons.HostApiOrtherPin;
            var path = string.Empty;
            if (!next_page)
            {
                var objJson = new
                {
                    options = new
                    {
                        field_set_key = "base_grid",
                        pin = pinId,
                        prepend = false,
                        search_query = search_str,
                        source = "search",
                        top_level_source = "search",
                        top_level_source_depth = 1,
                        context_pin_ids = new string[] { }
                    },
                    context = new
                    {
                    },

                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiOrtherPin + "/pin/" + pinId + "/";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }
            else
            {
                var objJson = new
                {
                    options = new
                    {
                        field_set_key = "base_grid",
                        pin = pinId,
                        prepend = false,
                        search_query = search_str,
                        source = "search",
                        top_level_source = "search",
                        top_level_source_depth = 1,
                        bookmarks = new string[] { bookmarks_str },
                        context_pin_ids = new string[] { }
                    },
                    context = new
                    {

                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiOrtherPin + "/pin/" + pinId + "/";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }

            // data = HttpContext.Current.Server.UrlEncode(data);
            var timestamp = GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "&_=" + timestamp;
            var bookmarks = "";
            getDataPinterest(url, model, pinId, ref bookmarks);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_OrtherPins(ref model, search_str, limit, bookmarks, ++page, pinId);
            }
            return false;
        }

        public static PinsModels getDataPinterestDetail(string url, string pinId, ref string bookmarks)
        {

            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = "_auth=1; csrftoken=dMWi2a6L1DTFUHmyqem0oGrDmteiaETw; _pinterest_sess=\"TWc9PSZsSlA1dUF4QWlRWGRYVGR6Qm9mN3pwczUyUDk4ZDYvckduSjl4N3ZSRHlsU1VmWkhBTUsrMU9KNkxjS3pyUk1zREdDL2Rmb2VuT1dwRDhSTmxTOE1Ja0FjOUtreTJVc0o0SmthQ2xhN3lRa3BQVnRMcUF5dlN0Z255Syt4am56VnQvYVQwT0JyejBCSlk4YzFyQ0pEekZwNSs0YjZnMTBseEIvRkU0Um1XeWthZ1cvNGxpdDVyTEdrSHRzWFVLN244T25TaGVoYy93TGVSRjVxNzl5dnlZV1A5L3NlNnc5MWE4djl0ZjNoeEhqTTNuaGduRnZ2VkF1RTd6V1V3VnBCT3cyMksxMHJIdVE0TVVjc3FmWVozVllzekhpNFRGNDFBTERIVzdkcUNUS3NlWEJFdE1mSXJBbnNPVStHQXJiUWJRSENyVVVKTVJYNit5MkZTMFVNN3ptY09FNmFoaHk3Nk9MdUtuRmdDSWRWRVhPTWYrSXA4dFhlRU1hYW5paFNQMU5OcFNwY2xSZlJHZVlWWU03eHFsNWVmSWRHL0ZtN3NhdU9ubzhpUjZqMzNTTUxwMTlOQWRGa29zVUc1UXFqZ1BUYzhHL3M0YndDY2ZBN2ZMZnJQZTlGbXdPWjg5SXJVOEpUMEtPVnMzcjZPcytOVHRFUnlRUnoyNmJZdjl0YXJlOVp1WGQvM29SSi9xWUwvYmFPcDl5VFl1aEw2ZFBtMHlhZ0g4MXlIMXp1dnFXWWY1VytmY0ZPc0FSMzhqYXdhNTBqQjlYRHJ6OE9CY1ViMmljZkFhQkVydGxyVUtlNis4cnh3R3NPbXVTVjZCZUNTR1NKQ3JpWFJsajBsSEFGcytOMnptN2R2S1BXN1NocTFtZVlKMzF0Y1hyQXNseG9DdzdrQklxNnZXMkk2dXQ4azJJOTR4YWlIUDMvVzAwcmQ0SDVqNnhYc3NlTTNpK0ZHUU9xaUpCOER0N1pQaWFFTUhLRGxpdk1EVDlOYi9DdmRLcTQvdUROekpjRXNJSjVtcEl1bWVLUHhRdTVQQk91L1RWS0w0YkkzZDNwaW5mRnJFakRsck9aNTRBUXVsVFdFWVlTRHJ5OUxBWHdMa0V4Jk1FSHZIUWlQUlE2Q05OZWJydEZrV25SQ2tmND0 = \"; G_ENABLED_IDPS=google; _b=\"ATWTNNfXaINNj5j6VvA6 + rquchpAz7VF + IS8VabE7fJo7ragqOV82ASwCOgxcnxHC5k = \"; pnodepath=\" / 4\"; _ga=GA1.2.1908176321.1528170001; fba=True; cm_sub=none; sessionFunnelEventLogged=1; bei=false";
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var results = (dynamic)null;
                                results = data;
                                if (results != null)
                                {
                                    var pin = new PinsModels();
                                    var itemPin = results as Dictionary<string, dynamic>;
                                    bool flag = true;
                                    if (itemPin.ContainsKey("domain"))
                                    {
                                        pin.Domain = itemPin["domain"];
                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                    if (itemPin.ContainsKey("id"))
                                    {
                                        pin.ID = itemPin["id"];
                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                    if (itemPin.ContainsKey("link"))
                                    {
                                        pin.Link = itemPin["link"];
                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                    if (itemPin.ContainsKey("created_at"))
                                    {
                                        pin.Created_At = DateTime.Parse(itemPin["created_at"], new CultureInfo("en-US", true));
                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                    if (itemPin.ContainsKey("images"))
                                    {
                                        var Images = itemPin["images"] as Dictionary<string, dynamic>;
                                        if (Images != null)
                                        {
                                            foreach (var itemImg in Images)
                                            {
                                                var Image = itemImg.Value;
                                                var _ImageModel = new ImageModels()
                                                {
                                                    url = Image["url"],
                                                    height = Convert.ToInt16(Image["height"]),
                                                    width = Convert.ToInt16(Image["width"])
                                                };
                                                pin.Images.Add(_ImageModel);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                    return pin;
                                }
                            }

                            var dataBookmark = resource_data_cache[0]["response"];
                            if (dataBookmark != null)
                            {
                                bookmarks = dataBookmark["bookmark"];
                            }
                        }
                    }

                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public static bool Get_Tagged_PinsDetail(ref PinsModels model, string pinId = "")
        {
            string data = string.Empty;
            var urlOrg = Commons.HostApiOrtherPin;
            var objJson = new
            {
                options = new
                {
                    field_set_key = "detailed",
                    id = pinId,
                    is_landing_page = false,
                },
                context = new
                {
                },

            };
            string input = JsonConvert.SerializeObject(objJson);
            urlOrg = Commons.HostApiPinDetail + "/pin/" + pinId + "/";
            string[] pattern = new string[] { "\n", "\r", "\t" };
            string[] replacements = new string[] { "", "", "" };
            data = Preg_replace(input, pattern, replacements);
            // data = HttpContext.Current.Server.UrlEncode(data);
            var timestamp = GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "&_=" + timestamp;
            var bookmarks = "";
            model = getDataPinterestDetail(url, pinId, ref bookmarks);
            return false;
        }

        public static bool Get_Tagged_HomePins(ref CMS_CrawlerModels model, int limit = 1, string bookmarks_str = null, int page = 1)
        {
            if (page > limit) return false;
            var next_page = false;
            if (!string.IsNullOrEmpty(bookmarks_str))
                next_page = true;

            string data = string.Empty;
            var urlOrg = Commons.HostApi;
            if (!next_page)
            {
                var objJson = new
                {
                    options = new
                    {
                        field_set_key = "hf_grid",
                        in_nux = false,
                        is_react = true,
                        prependPartner = false,
                        prependUserNews = false,
                        repeatRequestBookmark = "",
                        static_feed = false
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiHomePin;
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }
            else
            {
                var objJson = new
                {
                    options = new
                    {
                        bookmarks = new string[] { bookmarks_str },
                        field_set_key = "hf_grid",
                        in_nux = false,
                        is_react = true,
                        prependPartner = false,
                        prependUserNews = false,
                        repeatRequestBookmark = "",
                        static_feed = false
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiHomePin;
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = Preg_replace(input, pattern, replacements);
            }

            // data = HttpContext.Current.Server.UrlEncode(data);
            var timestamp = GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "&_=" + timestamp;
            var bookmarks = "";
            getDataPinterestHome(url, model, "", ref bookmarks);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_HomePins(ref model, limit, bookmarks, ++page);
            }
            return false;
        }

        public static CMS_CrawlerModels getDataPinterestHome(string url, CMS_CrawlerModels model, string pinId, ref string bookmarks)
        {

            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = "_auth=1; csrftoken=dMWi2a6L1DTFUHmyqem0oGrDmteiaETw; _pinterest_sess=\"TWc9PSZsSlA1dUF4QWlRWGRYVGR6Qm9mN3pwczUyUDk4ZDYvckduSjl4N3ZSRHlsU1VmWkhBTUsrMU9KNkxjS3pyUk1zREdDL2Rmb2VuT1dwRDhSTmxTOE1Ja0FjOUtreTJVc0o0SmthQ2xhN3lRa3BQVnRMcUF5dlN0Z255Syt4am56VnQvYVQwT0JyejBCSlk4YzFyQ0pEekZwNSs0YjZnMTBseEIvRkU0Um1XeWthZ1cvNGxpdDVyTEdrSHRzWFVLN244T25TaGVoYy93TGVSRjVxNzl5dnlZV1A5L3NlNnc5MWE4djl0ZjNoeEhqTTNuaGduRnZ2VkF1RTd6V1V3VnBCT3cyMksxMHJIdVE0TVVjc3FmWVozVllzekhpNFRGNDFBTERIVzdkcUNUS3NlWEJFdE1mSXJBbnNPVStHQXJiUWJRSENyVVVKTVJYNit5MkZTMFVNN3ptY09FNmFoaHk3Nk9MdUtuRmdDSWRWRVhPTWYrSXA4dFhlRU1hYW5paFNQMU5OcFNwY2xSZlJHZVlWWU03eHFsNWVmSWRHL0ZtN3NhdU9ubzhpUjZqMzNTTUxwMTlOQWRGa29zVUc1UXFqZ1BUYzhHL3M0YndDY2ZBN2ZMZnJQZTlGbXdPWjg5SXJVOEpUMEtPVnMzcjZPcytOVHRFUnlRUnoyNmJZdjl0YXJlOVp1WGQvM29SSi9xWUwvYmFPcDl5VFl1aEw2ZFBtMHlhZ0g4MXlIMXp1dnFXWWY1VytmY0ZPc0FSMzhqYXdhNTBqQjlYRHJ6OE9CY1ViMmljZkFhQkVydGxyVUtlNis4cnh3R3NPbXVTVjZCZUNTR1NKQ3JpWFJsajBsSEFGcytOMnptN2R2S1BXN1NocTFtZVlKMzF0Y1hyQXNseG9DdzdrQklxNnZXMkk2dXQ4azJJOTR4YWlIUDMvVzAwcmQ0SDVqNnhYc3NlTTNpK0ZHUU9xaUpCOER0N1pQaWFFTUhLRGxpdk1EVDlOYi9DdmRLcTQvdUROekpjRXNJSjVtcEl1bWVLUHhRdTVQQk91L1RWS0w0YkkzZDNwaW5mRnJFakRsck9aNTRBUXVsVFdFWVlTRHJ5OUxBWHdMa0V4Jk1FSHZIUWlQUlE2Q05OZWJydEZrV25SQ2tmND0 = \"; G_ENABLED_IDPS=google; _b=\"ATWTNNfXaINNj5j6VvA6 + rquchpAz7VF + IS8VabE7fJo7ragqOV82ASwCOgxcnxHC5k = \"; pnodepath=\" / 4\"; _ga=GA1.2.1908176321.1528170001; fba=True; cm_sub=none; sessionFunnelEventLogged=1; bei=false";
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_response"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache["data"];
                            if (data != null)
                            {
                                var results = (dynamic)null;
                                results = data;
                                if (results != null)
                                {
                                    foreach (var item in results)
                                    {
                                        var pin = new PinsModels();
                                        var itemPin = item as Dictionary<string, dynamic>;
                                        bool flag = true;
                                        if (itemPin.ContainsKey("domain"))
                                        {
                                            pin.Domain = itemPin["domain"];
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("id"))
                                        {
                                            pin.ID = itemPin["id"];
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("link"))
                                        {
                                            pin.Link = itemPin["link"];
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("created_at"))
                                        {
                                            pin.Created_At = DateTime.Parse(itemPin["created_at"], new CultureInfo("en-US", true));
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (itemPin.ContainsKey("images"))
                                        {
                                            var Images = itemPin["images"] as Dictionary<string, dynamic>;
                                            if (Images != null)
                                            {
                                                foreach (var itemImg in Images)
                                                {
                                                    var Image = itemImg.Value;
                                                    var _ImageModel = new ImageModels()
                                                    {
                                                        url = Image["url"],
                                                        height = Convert.ToInt16(Image["height"]),
                                                        width = Convert.ToInt16(Image["width"])
                                                    };
                                                    pin.Images.Add(_ImageModel);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                        if (flag)
                                            model.Pins.Add(pin);
                                    }
                                }
                            }

                            var dataBookmark = dobj["resource"]["options"];
                            if (dataBookmark != null)
                            {
                                bookmarks = dataBookmark["bookmarks"][0];
                            }
                        }
                    }

                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex) { }
            return model;
        }
        
        public static int getRePinCount(string url, string pinId, ref string bookmarks)
        {

            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = "_auth=1; csrftoken=dMWi2a6L1DTFUHmyqem0oGrDmteiaETw; _pinterest_sess=\"TWc9PSZsSlA1dUF4QWlRWGRYVGR6Qm9mN3pwczUyUDk4ZDYvckduSjl4N3ZSRHlsU1VmWkhBTUsrMU9KNkxjS3pyUk1zREdDL2Rmb2VuT1dwRDhSTmxTOE1Ja0FjOUtreTJVc0o0SmthQ2xhN3lRa3BQVnRMcUF5dlN0Z255Syt4am56VnQvYVQwT0JyejBCSlk4YzFyQ0pEekZwNSs0YjZnMTBseEIvRkU0Um1XeWthZ1cvNGxpdDVyTEdrSHRzWFVLN244T25TaGVoYy93TGVSRjVxNzl5dnlZV1A5L3NlNnc5MWE4djl0ZjNoeEhqTTNuaGduRnZ2VkF1RTd6V1V3VnBCT3cyMksxMHJIdVE0TVVjc3FmWVozVllzekhpNFRGNDFBTERIVzdkcUNUS3NlWEJFdE1mSXJBbnNPVStHQXJiUWJRSENyVVVKTVJYNit5MkZTMFVNN3ptY09FNmFoaHk3Nk9MdUtuRmdDSWRWRVhPTWYrSXA4dFhlRU1hYW5paFNQMU5OcFNwY2xSZlJHZVlWWU03eHFsNWVmSWRHL0ZtN3NhdU9ubzhpUjZqMzNTTUxwMTlOQWRGa29zVUc1UXFqZ1BUYzhHL3M0YndDY2ZBN2ZMZnJQZTlGbXdPWjg5SXJVOEpUMEtPVnMzcjZPcytOVHRFUnlRUnoyNmJZdjl0YXJlOVp1WGQvM29SSi9xWUwvYmFPcDl5VFl1aEw2ZFBtMHlhZ0g4MXlIMXp1dnFXWWY1VytmY0ZPc0FSMzhqYXdhNTBqQjlYRHJ6OE9CY1ViMmljZkFhQkVydGxyVUtlNis4cnh3R3NPbXVTVjZCZUNTR1NKQ3JpWFJsajBsSEFGcytOMnptN2R2S1BXN1NocTFtZVlKMzF0Y1hyQXNseG9DdzdrQklxNnZXMkk2dXQ4azJJOTR4YWlIUDMvVzAwcmQ0SDVqNnhYc3NlTTNpK0ZHUU9xaUpCOER0N1pQaWFFTUhLRGxpdk1EVDlOYi9DdmRLcTQvdUROekpjRXNJSjVtcEl1bWVLUHhRdTVQQk91L1RWS0w0YkkzZDNwaW5mRnJFakRsck9aNTRBUXVsVFdFWVlTRHJ5OUxBWHdMa0V4Jk1FSHZIUWlQUlE2Q05OZWJydEZrV25SQ2tmND0 = \"; G_ENABLED_IDPS=google; _b=\"ATWTNNfXaINNj5j6VvA6 + rquchpAz7VF + IS8VabE7fJo7ragqOV82ASwCOgxcnxHC5k = \"; pnodepath=\" / 4\"; _ga=GA1.2.1908176321.1528170001; fba=True; cm_sub=none; sessionFunnelEventLogged=1; bei=false";
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var results = (dynamic)null;
                                results = data;
                                if (results != null)
                                {
                                    var itemPin = results as Dictionary<string, dynamic>;
                                    if (itemPin.ContainsKey("repin_count"))
                                    {
                                        var _Repin_Count = Convert.ToInt16(itemPin["repin_count"]);
                                        return _Repin_Count;
                                    }
                                }
                            }

                            var dataBookmark = resource_data_cache[0]["response"];
                            if (dataBookmark != null)
                            {
                                bookmarks = dataBookmark["bookmark"];
                            }
                        }
                    }

                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex) { }
            return 0;
        }

        public static bool Get_Tagged_RepinCount(ref int RepinCount, string pinId = "")
        {
            string data = string.Empty;
            var urlOrg = Commons.HostApiPinDetail;
            var objJson = new
            {
                options = new
                {
                    field_set_key = "detailed",
                    id = pinId,
                    is_landing_page = false,
                },
                context = new
                {
                },

            };
            string input = JsonConvert.SerializeObject(objJson);
            urlOrg = Commons.HostApiPinDetail + "/pin/" + pinId + "/";
            string[] pattern = new string[] { "\n", "\r", "\t" };
            string[] replacements = new string[] { "", "", "" };
            data = Preg_replace(input, pattern, replacements);
            var timestamp = GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "&_=" + timestamp;
            var bookmarks = "";
            RepinCount = getRePinCount(url, pinId, ref bookmarks);
            return false;
        }

    }
}
