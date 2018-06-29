using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GuestTours.Models;

namespace GuestTours.Controllers
{
    public class BaseController : Controller
    {
        GuestToursEntities db = new GuestToursEntities();
        protected string Language { get; set; }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.RouteData.Values.ContainsKey("language"))
                Language = filterContext.RouteData.Values["language"].ToString();
            else
                Language = "en-US";

            ViewBag.language = Language;
            if (db.Languages.Any(x => x.LangCode == Language))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ka-GE");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ka-GE");
            }

           
             
            
        }
    }
}