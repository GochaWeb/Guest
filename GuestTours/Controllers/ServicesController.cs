using GuestTours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Controllers
{
    public class ServicesController : BaseController
    {
        GuestToursEntities db;
        void CheckConnection()
        {
            if (db == null)
            {
                db = new GuestToursEntities();
            }
        }

        public ActionResult View(TourModel tourmodel)
        {
            CheckConnection();

            if (db.Languages.Any(x => x.LangCode == Language))
            {
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();

               // ViewBag.Category = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();
                if (tourmodel.by != null)
                {

                    var model = db.ServicesTranslateds.Where(x => x.CategoryName == tourmodel.by).ToList();
                    return View(model);
                }

                return View(db.ServicesTranslateds.Where(x => x.LangCode == Language).ToList());
            }
            else
            {
                if(tourmodel.by != null)
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    var model = db.ServicesTranslateds.Where(x => x.CategoryName == tourmodel.by &&x.LangCode=="ka-GE").ToList();
                    return View(model);

                }
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
               // ViewBag.Category = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                return View(db.ServicesTranslateds.Where(x => x.LangCode == "ka-GE").ToList());
            }
        }

        public ActionResult Details(TourModel tourmodel)
        {
            CheckConnection();
            if (db.ServicesTranslateds.Any(x => x.TourID == tourmodel.Id))
            {
                if (db.Languages.Any(x => x.LangCode == Language))
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();
                    var model = db.ServicesTranslateds.Where(x => x.TourID == tourmodel.Id && x.LangCode == Language).FirstOrDefault();
                    ViewBag.Active = db.ServicesPhotoes.Where(x => x.TourID == tourmodel.Id).FirstOrDefault().Photo;
                    ViewBag.Slider = db.ServicesPhotoes.Where(x => x.TourID == tourmodel.Id).ToList();
                    return View(model);

                }
                else
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    var model = db.ServicesTranslateds.Where(x => x.TourID == tourmodel.Id && x.LangCode == "ka-GE").FirstOrDefault();
                    ViewBag.Active = db.ServicesPhotoes.Where(x => x.TourID == tourmodel.Id).FirstOrDefault().Photo;
                    ViewBag.Slider = db.ServicesPhotoes.Where(x => x.TourID == tourmodel.Id).ToList();
                    return View(model);

                }
               
            }
            else
            {
                return RedirectToAction("View");
            }
           
        }

        public JsonResult Get(string Id, string url)
        {
            CheckConnection();
            if (!string.IsNullOrEmpty(Id) && Id != null)
            {
                int ID = db.CategoryServiceTranslateds.Where(x => x.Name == Id).FirstOrDefault().CategoryID;
                string name = db.CategoryServiceTranslateds.Where(x => x.CategoryID == ID && x.LangCode == url).FirstOrDefault().Name;
                return Json(name);
            }
            else
            {
                return Json(false);
            }
        }

    }
}