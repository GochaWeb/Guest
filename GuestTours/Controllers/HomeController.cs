using GuestTours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Controllers
{
    public class HomeController : BaseController
    {
        GuestToursEntities db;
        void CheckConnection()
        {
            if (db == null)
            {
                db = new GuestToursEntities();
            }
        }
        public ActionResult Index()
        {
            CheckConnection();
            if (db.Languages.Any(x => x.LangCode == Language))
            {
                ViewBag.Slider = db.SliderTranslateds.Where(x => x.LangCode == Language).ToList();
              
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();
                ViewBag.Tours = db.TourTranslateds.Where(x => x.LangCode == Language && x.Tour.Active == true).ToList();
                ViewBag.Service = db.ServicesTranslateds.Where(x => x.LangCode == Language).ToList();
                return View(db.TourTranslateds.Where(x=>x.LangCode==Language).ToList());
            }
            ViewBag.Slider = db.SliderTranslateds.Where(x => x.LangCode == "ka-GE").ToList();

            ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
            ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
            ViewBag.Tours = db.TourTranslateds.Where(x => x.LangCode == "ka-GE" && x.Tour.Active == true).ToList();
            ViewBag.Service = db.ServicesTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
            return View(db.TourTranslateds.Where(x => x.LangCode == "ka-GE"));
        }
    }
}