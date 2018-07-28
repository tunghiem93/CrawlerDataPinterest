using CMS_DTO.CMSBoard;
using CMS_DTO.CMSCrawler;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSBoardController : Controller
    {
        // GET: Admin/CMSBoard
        public ActionResult Index()
        {
            try
            {
                List<CMS_BoardModels> models = new List<CMS_BoardModels>();
                CrawlerBoardHelper._Cookies = "G_ENABLED_IDPS=google; _b=\"ATW1MZ0GSMJI46hOmoQ3MpWpXe6+rP8fkEoLnaSC6XQ7dd54Cm+VLWXBJOy2qLRza8E=\"; _auth=1; csrftoken=g9vz7YbodrSvIMzOaDUOcDLGNhdKJP1v; _pinterest_sess=\"TWc9PSZsVVROcGxoeVNLOE1TK2h6UVlDcU1JS0JqMkxIb2M3Vi9OMzYvMEZydnEzOGdKYzh3M0tsN3hKRndUeVBzK1RmQXhsUmNVTkRpN0h4S21HTHRpeWRHd2ZSVGlHMWhoenBVZTFDYWdZdFBKK3lNOFlrSkt3MmVmOE1qUGJ6cmFtbitqQzZTSDJKenRhM0JpTmhGU1pRaFZ6NXc0MlVkcTlZdDJIYnlsenNvMlYrMmlkam5VVEVPTnhEQmxHcUxWNTFUTWc4amxQVnpBOEtWM29TVTV1K0JWK3pjNFpseXVTNGhTNTN4NUQ4QTRvdUYwdWo4ZzJDdFpjRE1PazdTVFJwcVM2UnVGVzl5U2lXZldic2xlUVdMdFBqK3hTTXJqazl2R21ibjYyMFhUYkYxR2xRMjk4ZG9ZKzI4ck85TGVaUUNVMzVDZUk3R285eHk0bnEyVmEwSkNLMmp2ZjlGSWtGbzJ6QXV6Y0NnU0FHZm9vQ292RklwRE5tSGZtSkRPTHEreU81VnkvbkZoUXA5SXRkRmNLNXkxUktPNXhFVDlBNHpJSkZ3T1dmSUdqaElJbmFOS2pIcVpFam44WjlicEU2T3NqbnRGUkx3UkVsRzA5RDI3SGI4NFVrK3c4S2FjaU9UbCtvOHJDVU9JU2JjNndOK2lLWkVlQ1lpcHhDOXdIY2FSY1Z4NXNtR1dqV1g0aTA0WURzSEtOOWdHcFpld2t2ZGxscTh6RDY2SHpYVFFiM2JFU0FYNzZndkJ0OTliY2MyRHhTWGVsWDFHQkd6NXlZbnVzQkkyOHJxNEdyQWhoVDRJaXAzQTBhNGJMbUJ1RXRwUTNIZDlKZi9UcFBrQm5ZRlpsRlJFY3NuUnJFYUdEZEQvYUlOVXUyNGUvaURiQXQ1ZDZKZU9DMXJtV3NWakdyazdLSnZrMndiWE1uSmM5UncrN0hvTWwwN1g4azFUVUM2R21LUFN5Z1IvSVhxSmVPYm81QWo5R1VVRDdycUJxWnpsZWFvcVpCOHVpK3JnVmYwTHlNU0hRNEJwak1EYkVTMS8yRnd3VlBYNXlDNmI5Y2s2clFxa3hLcFJmODFjT0s3RXk0dEJuWTl4ZTdWU1BBJkF4Tm5NSWQ0UUo3RW9nSzJkUWVDaXJ1a1JOTT0=\"; _ga=GA1.2.729252838.1528255220; cm_sub=none; sessionFunnelEventLogged=1; bei=false";
                CrawlerBoardHelper.Get_Tagged_Board(ref models, "Footballs", 2, null);
            }
            catch(Exception ex)
            {
                NSLog.Logger.Error("CMSBoard_Index", ex);
            }
            return View();
        }
    }
}