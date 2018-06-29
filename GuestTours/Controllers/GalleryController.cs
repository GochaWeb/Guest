using GuestTours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Controllers
{
    public class GalleryController : BaseController
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
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();

            }
            else
            {
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
            }
           

            return View(db.Galleries.ToList());
        }
    }
}