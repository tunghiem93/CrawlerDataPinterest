using CMS_DTO.CMSBoard;
using CMS_DTO.CMSCrawler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CMS_Shared.Utilities
{
    public class CrawlerBoardHelper
    {
        public static string _Cookies { get; set; }
        public static bool Get_Tagged_Board(ref List<CMS_BoardModels> model, string search_str, int limit = 1, string bookmarks_str = null, int page = 1)
        {
            if (page > limit) return false;
            var next_page = false;
            if (!string.IsNullOrEmpty(bookmarks_str))
                next_page = true;

            string data = string.Empty;
            var urlOrg = Commons.HostApiBoard + search_str;
            var path = string.Empty;
            if (!next_page)
            {
                var objJson = new
                {
                    options = new
                    {
                        auto_correction_disabled = false,
                        query = search_str,
                        redux_normalize_feed = false,
                        rs = "filter",
                        scope = "boards",
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiBoard + search_str + "&rs=filter";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = InitBaseBoard.Preg_replace(input, pattern, replacements);
            }
            else
            {
                var objJson = new
                {
                    options = new
                    {
                        auto_correction_disabled = false,
                        query = search_str,
                        redux_normalize_feed = false,
                        rs = "filter",
                        scope = "boards",
                        bookmarks = new string[] { bookmarks_str },
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiBoard + search_str + "&rs=filter";
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = InitBaseBoard.Preg_replace(input, pattern, replacements);
            }
            var timestamp = InitBaseBoard.GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "" + path + "&_=" + timestamp;
            var bookmarks = "";
            getDataBoard(url, ref model, ref bookmarks);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_Board(ref model, search_str, limit, bookmarks, ++page);
            }
            return false;
        }

        /* parse data board from api */
        public static void getDataBoard(string url, ref List<CMS_BoardModels> model, ref string bookmarks)
        {
            dynamic dataLog = null;
            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = _Cookies;
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    dataLog = dobj;
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var results = (dynamic)null;
                                results = data["results"];

                                if (results != null)
                                {
                                    var json = new JavaScriptSerializer().Serialize(results);
                                    List<CMS_BoardModels> objBoards = JsonConvert.DeserializeObject<List<CMS_BoardModels>>(json);
                                    if (objBoards != null)
                                        model.AddRange(objBoards);
                                }
                            }
                            var dataBookmark = resource_data_cache[0]["response"];
                            if (dataBookmark != null)
                            {
                                if (dataBookmark.ContainsKey("bookmark"))
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
                NSLog.Logger.Info("ErrorGetDataPinterest: ", dataLog);
                NSLog.Logger.Info("ErrorGetDataPinterest: ", ex);
            }
        }


        public static bool Get_Tagged_PinOfBoard(ref List<CMS_PinOfBoardModels> model, string board_url, string boardId, int limit = 1, string bookmarks_str = null, int page = 1)
        {
            if (page > limit) return false;
            var next_page = false;
            if (!string.IsNullOrEmpty(bookmarks_str))
                next_page = true;

            string data = string.Empty;
            var urlOrg = Commons.HostApiPinOfBoard + board_url;
            var path = string.Empty;
            if (!next_page)
            {
                var objJson = new
                {
                    options = new
                    {
                        board_id = boardId,
                        board_url= board_url,
                        field_set_key = "react_grid_pin",
                        filter_section_pins = true,
                        layout = "default",
                        page_size = 25,
                        redux_normalize_feed = true,
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiPinOfBoard + board_url;
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = InitBaseBoard.Preg_replace(input, pattern, replacements);
            }
            else
            {
                var objJson = new
                {
                    options = new
                    {
                        board_id = boardId,
                        board_url = board_url,
                        field_set_key = "react_grid_pin",
                        filter_section_pins = true,
                        layout = "default",
                        page_size = 25,
                        redux_normalize_feed = true,
                        bookmarks = new string[] { bookmarks_str },
                    },
                    context = new
                    {
                    },
                };
                string input = JsonConvert.SerializeObject(objJson);
                urlOrg = Commons.HostApiPinOfBoard + board_url;
                path = "";
                string[] pattern = new string[] { "\n", "\r", "\t" };
                string[] replacements = new string[] { "", "", "" };
                data = InitBaseBoard.Preg_replace(input, pattern, replacements);
            }
            var timestamp = InitBaseBoard.GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "" + path + "&_=" + timestamp;
            var bookmarks = "";
            getDataPinOfBoard(url, ref model, ref bookmarks);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_PinOfBoard(ref model, board_url, boardId, limit, bookmarks, ++page);
            }
            return false;
        }

        /* parse data pin of board from api */
        public static void getDataPinOfBoard(string url, ref List<CMS_PinOfBoardModels> model, ref string bookmarks)
        {
            dynamic dataLog = null;
            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = _Cookies;
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    dataLog = dobj;
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var json = new JavaScriptSerializer().Serialize(data);
                                List<CMS_PinOfBoardModels> objBoards = JsonConvert.DeserializeObject<List<CMS_PinOfBoardModels>>(json);
                                if (objBoards != null)
                                    model.AddRange(objBoards);
                            }
                            var dataBookmark = resource_data_cache[0]["response"];
                            if (dataBookmark != null)
                            {
                                if (dataBookmark.ContainsKey("bookmark"))
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
                NSLog.Logger.Info("ErrorGetDataPinterest: ", dataLog);
                NSLog.Logger.Info("ErrorGetDataPinterest: ", ex);
            }
        }


        public static bool Get_Tagged_PinDetail(ref CMS_PinOfBoardModels model, string pinId)
        {
            string data = string.Empty;
            var urlOrg = Commons.HostApiPinDetail + "pin/"+pinId;
            var path = string.Empty;
            var objJson = new
            {
                options = new
                {
                    id = pinId,
                    field_set_key = "detailed",
                    is_landing_page = false,
                },
                context = new
                {
                },
            };
            string input = JsonConvert.SerializeObject(objJson);
            path = "";
            string[] pattern = new string[] { "\n", "\r", "\t" };
            string[] replacements = new string[] { "", "", "" };
            data = InitBaseBoard.Preg_replace(input, pattern, replacements);
            var timestamp = InitBaseBoard.GetTimestamp(DateTime.Now);
            var url = urlOrg + "&data=" + data + "" + path + "&_=" + timestamp;
            var bookmarks = "";
            getDataPinDetail(url, ref model);

            if (!string.IsNullOrEmpty(bookmarks))
            {
                Get_Tagged_PinDetail(ref model, pinId);
            }
            return false;
        }

        /* parse data pin of board from api */
        public static void getDataPinDetail(string url, ref CMS_PinOfBoardModels model)
        {
            dynamic dataLog = null;
            try
            {
                Uri uri = new Uri(url);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.Headers["Cookie"] = _Cookies;
                httpWebRequest.Timeout = 100000;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var answer = streamReader.ReadToEnd();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(answer);
                    dataLog = dobj;
                    if (dobj != null)
                    {
                        var resource_data_cache = dobj["resource_data_cache"];
                        if (resource_data_cache != null)
                        {
                            var data = resource_data_cache[0]["data"];
                            if (data != null)
                            {
                                var json = new JavaScriptSerializer().Serialize(data);
                                CMS_PinOfBoardDetailModels objBoards = JsonConvert.DeserializeObject<CMS_PinOfBoardDetailModels>(json);
                                if (objBoards != null)
                                    model.created_at = DateTime.Parse(objBoards.created_at.ToString(), new CultureInfo("en-US", true)); ;
                            }
                        }
                    }
                    streamReader.Close();
                    streamReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Info("ErrorGetDataPinterest: ", dataLog);
                NSLog.Logger.Info("ErrorGetDataPinterest: ", ex);
            }
        }
    }

    public static class InitBaseBoard
    {
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
    }
}
