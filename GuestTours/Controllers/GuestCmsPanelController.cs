using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuestTours.Models;
using GuestTours.Helper;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace GuestTours.Controllers
{
    public class GuestCmsPanelController : Controller
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
            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        public ActionResult Category(CategoryModel model)
        {
            CheckConnection();
            db.Categories.Add(new Models.Category()
            {
                 CreateDate=DateTime.Now,
                 
            });
            db.SaveChanges();
            var category = db.Categories.OrderByDescending(x => x.Id).FirstOrDefault();
            category.CategoryTranslateds.Add(new CategoryTranslated()
            {
                CategoryID=category.Id,
                LangCode = model.LangEng,
                Name=model.NameEng
            });
            db.SaveChanges();
            category.CategoryTranslateds.Add(new CategoryTranslated()
            {
                CategoryID = category.Id,
                LangCode = model.LangGeo,
                Name = model.NameGeo,
            });
            db.SaveChanges();
            category.CategoryTranslateds.Add(new CategoryTranslated()
            {
                CategoryID = category.Id,
                LangCode = model.LangRus,
                Name = model.NameRus,
            });
            db.SaveChanges();




            TempData["Success"] = "წარმატებით დაემატა";
            return RedirectToAction("AddCategory");
        }

        public ActionResult Tours()
        {
            CheckConnection();
            ViewBag.CategoryEng = db.CategoryTranslateds.Where(x => x.LangCode == "en-US").ToList();
            ViewBag.CategoryRus = db.CategoryTranslateds.Where(x => x.LangCode == "ru-RU").ToList();
            ViewBag.CategoryGeo = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
            return View();
        }

        public ActionResult AddTour(TourModel model,HttpPostedFileBase Photo)
        {
            CheckConnection();
            string name = MainHelper.Random32();
            db.Tours.Add(new Tour()
            {
                 TourDate=model.TourDate,
                 Price=model.Price,
                 Photo="/images/Tours/"+name+".jpeg",
                 Active=false,
                 CreateDate=DateTime.Now,

            });
            db.SaveChanges();
            using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 1000, 1000))
            {

                string path = Server.MapPath("/images/Tours/" + name + ".jpeg");

                newimage.Save(path, ImageFormat.Jpeg);

            }
            var Id = db.Tours.Max(x => x.Id);
            db.TourTranslateds.Add(new TourTranslated()
            {
                TourID=Id,
                LangCode=model.LangEng,
                CategoryName=model.CategoryNameEng,
                Title=model.TitleEng,
                Text=model.TextEng,
                BigText=model.BigTextEng,
            });

            db.TourTranslateds.Add(new TourTranslated()
            {
                TourID = Id,
                LangCode = model.LangRus,
                CategoryName = model.CategoryNameRus,
                Title = model.TitleRus,
                Text = model.TextRus,
                BigText = model.BigTextRus,
            });

            db.TourTranslateds.Add(new TourTranslated()
            {
                TourID = Id,
                LangCode = model.LangGeo,
                CategoryName = model.CategoryNameGeo,
                Title = model.TitleGeo,
                Text = model.TextGeo,
                BigText = model.BigTextGeo,
            });
            db.SaveChanges();

            TempData["Success"] = "წარმატებით დაემატა გთხოვთ დაამატოთ ტურის სლაიდერის ფოტოები";
            return RedirectToAction("Tours");
        }

        //ტურების შეცვლა წაშლა
        public ActionResult changeTours()
        {
            CheckConnection();

            return View(db.TourTranslateds.ToList());
        }

        public ActionResult ChangeAddedTours(int id=0)
        {
            CheckConnection();
            if (db.TourTranslateds.Any(x => x.TourID == id))
            {
                ViewBag.Tour = db.Tours.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.ToursEng = db.TourTranslateds.Where(x => x.TourID == id && x.LangCode=="en-US").FirstOrDefault();
                ViewBag.ToursRus = db.TourTranslateds.Where(x => x.TourID == id && x.LangCode=="ru-RU").FirstOrDefault();
                ViewBag.ToursGeo = db.TourTranslateds.Where(x => x.TourID == id && x.LangCode=="ka-GE").FirstOrDefault();
                ViewBag.Slider = db.TourPhotoes.Where(x => x.TourID == id).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("changeTours");
            }

            
        }
        [HttpPost]
        public ActionResult ChangeAddedTours(TourModel model,int Id,HttpPostedFileBase Photo)
        {
            CheckConnection();
            var Eng = db.TourTranslateds.Where(x => x.TourID == Id && x.LangCode == "en-US").FirstOrDefault();
            var Rus = db.TourTranslateds.Where(x => x.TourID == Id && x.LangCode == "ru-RU").FirstOrDefault();
            var Geo = db.TourTranslateds.Where(x => x.TourID == Id && x.LangCode == "ka-GE").FirstOrDefault();
            Eng.Tour.TourDate = model.TourDate;
            Eng.Tour.Price = model.Price;
            if(Photo != null)
            {
                string name = MainHelper.Random32();
                string Pname = Eng.Tour.Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                Eng.Tour.Photo = "/images/Tours/" + name + ".jpeg";
                using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 1000, 1000))
                {

                    string path = Server.MapPath("/images/Tours/" + name + ".jpeg");

                    newimage.Save(path, ImageFormat.Jpeg);

                }

            }
            Eng.CategoryName = model.CategoryNameEng;
            Eng.Title = model.TitleEng;
            Eng.Text = model.TextEng;
            Eng.BigText = model.BigTextEng;
            db.SaveChanges();
            Rus.CategoryName = model.CategoryNameRus;
            Rus.Title = model.TitleRus;
            Rus.Text = model.TextRus;
            Rus.BigText = model.BigTextRus;
            db.SaveChanges();
            Geo.CategoryName = model.CategoryNameGeo;
            Geo.Title = model.TitleGeo;
            Geo.Text = model.TextGeo;
            Geo.BigText = model.BigTextGeo;
            db.SaveChanges();

            TempData["Success"] = "წარმატებით შეიცვალა";
            return RedirectToAction("changeTours");
        }

        public ActionResult AddTourPhoto(IEnumerable<HttpPostedFileBase>Photo,int Id)
        {
            CheckConnection();
            foreach(var item in Photo)
            {
                if(item != null)
                {
                    string name = MainHelper.Random32();
                    db.TourPhotoes.Add(new TourPhoto()
                    {
                        TourID=Id,
                        Photo= "/images/TourSlider/" + name + ".jpeg",
                    });
                    using (var newimage = ScaleImage(Image.FromStream(item.InputStream, true, true), 1000, 1000))
                    {

                        string path = Server.MapPath("/images/TourSlider/" + name + ".jpeg");

                        newimage.Save(path, ImageFormat.Jpeg);

                    }
                    db.SaveChanges();

                }
            }
            TempData["Success"] = "წარმატებით დაემატა სლაიდერის ფოტოები";
            return RedirectToAction("ChangeAddedTours", new { id = Id });
        }

        public ActionResult DelPhoto(int id)
        {
            CheckConnection();
            if (db.TourPhotoes.Any(x => x.Id == id))
            {
                var ID = db.TourPhotoes.Where(x => x.Id == id).FirstOrDefault().TourID;
                string Pname = db.TourPhotoes.Where(x => x.Id == id).FirstOrDefault().Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                db.TourPhotoes.Remove(db.TourPhotoes.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();
                TempData["Success"] = "წარმატებით წაიშალა";
                return RedirectToAction("ChangeAddedTours", new { id = ID });

            }
            else
            {
                return RedirectToAction("changeTours");
            }
            
        }
        public ActionResult DeleteTour(int id)
        {
            CheckConnection();
            if (db.Tours.Any(x => x.Id == id))
            {
                foreach(var item in db.TourPhotoes.Where(x => x.TourID == id))
                {
                    string name = item.Photo;
                    string fullpath = Request.MapPath(name);
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                }
                string cover = db.Tours.Where(x => x.Id == id).FirstOrDefault().Photo;
                string full = Request.MapPath(cover);
                if (System.IO.File.Exists(full))
                {
                    System.IO.File.Delete(full);
                }
                db.TourPhotoes.RemoveRange(db.TourPhotoes.Where(x => x.TourID == id));
                db.TourTranslateds.RemoveRange(db.TourTranslateds.Where(x => x.TourID == id));
                db.Tours.Remove(db.Tours.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();

                TempData["Success"] = "წარმატებით წაიშალა";
                return RedirectToAction("changeTours");
            }
            else
            {
                return RedirectToAction("changeTours");
            }
        }

        public ActionResult MakePopular(int id)
        {
            CheckConnection();
            if (db.Tours.Any(x => x.Id == id))
            {
                db.Tours.Where(x => x.Id == id).FirstOrDefault().Active = true;
                db.SaveChanges();
                var name = db.TourTranslateds.Where(x => x.TourID == id && x.LangCode == "ka-GE").FirstOrDefault().Title;
                TempData["Success"] = name + " გახდა პოპულარული ტური";
            }
            return RedirectToAction("changeTours");
        }

        public ActionResult DelPopular(int id)
        {
            CheckConnection();
            if (db.Tours.Any(x => x.Id == id))
            {
                db.Tours.Where(x => x.Id == id).FirstOrDefault().Active = false;
                db.SaveChanges();
                var name = db.TourTranslateds.Where(x => x.TourID == id && x.LangCode == "ka-GE").FirstOrDefault().Title;
                TempData["Success"] = name + " წაიშალა პოპულარული ტურებიდან";
            }
            return RedirectToAction("changeTours");
        }

        //სერვისების დამატება

        public ActionResult ServicesCategory()
        {
            return View();
        }

        public ActionResult AddServCategory(CategoryModel model)
        {
            CheckConnection();
            db.CategoryServices.Add(new CategoryService()
            {
                CreateDate = DateTime.Now,

            });
            db.SaveChanges();
            var category = db.CategoryServices.OrderByDescending(x => x.Id).FirstOrDefault();
            category.CategoryServiceTranslateds.Add(new CategoryServiceTranslated()
            {
                CategoryID = category.Id,
                LangCode = model.LangEng,
                Name = model.NameEng
            });
            db.SaveChanges();
            category.CategoryServiceTranslateds.Add(new CategoryServiceTranslated()
            {
                CategoryID = category.Id,
                LangCode = model.LangGeo,
                Name = model.NameGeo,
            });
            db.SaveChanges();
            category.CategoryServiceTranslateds.Add(new CategoryServiceTranslated()
            {
                CategoryID = category.Id,
                LangCode = model.LangRus,
                Name = model.NameRus,
            });
            db.SaveChanges();




            TempData["Success"] = "წარმატებით დაემატა";
            return RedirectToAction("ServicesCategory");
        }
        
        public ActionResult AddServices()
        {
            CheckConnection();
            ViewBag.CategoryEng = db.CategoryServiceTranslateds.Where(x => x.LangCode == "en-US").ToList();
            ViewBag.CategoryRus = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ru-RU").ToList();
            ViewBag.CategoryGeo = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult AddServices(TourModel model,HttpPostedFileBase Photo)
        {
            CheckConnection();
            string name = MainHelper.Random32();
            db.Services.Add(new Service()
            {
                TourDate = model.TourDate,
                Price = model.Price,
                Photo = "/images/Services/" + name + ".jpeg",
                Active = false,
                CreateDate = DateTime.Now,

            });
            db.SaveChanges();
            using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 1000, 1000))
            {

                string path = Server.MapPath("/images/Services/" + name + ".jpeg");

                newimage.Save(path, ImageFormat.Jpeg);

            }
            var Id = db.Services.Max(x => x.Id);
            db.ServicesTranslateds.Add(new ServicesTranslated()
            {
                TourID = Id,
                LangCode = model.LangEng,
                CategoryName = model.CategoryNameEng,
                Title = model.TitleEng,
                Text = model.TextEng,
                BigText = model.BigTextEng,
            });

            db.ServicesTranslateds.Add(new ServicesTranslated()
            {
                TourID = Id,
                LangCode = model.LangRus,
                CategoryName = model.CategoryNameRus,
                Title = model.TitleRus,
                Text = model.TextRus,
                BigText = model.BigTextRus,
            });

            db.ServicesTranslateds.Add(new ServicesTranslated()
            {
                TourID = Id,
                LangCode = model.LangGeo,
                CategoryName = model.CategoryNameGeo,
                Title = model.TitleGeo,
                Text = model.TextGeo,
                BigText = model.BigTextGeo,
            });
            db.SaveChanges();

            TempData["Success"] = "წარმატებით დაემატა გთხოვთ დაამატოთ სერვისის სლაიდერის ფოტოები";
            return RedirectToAction("AddServices");
        }

        //სერვისების შეცვლა წაშლა
        public ActionResult ChnageService()
        {
            CheckConnection();

            return View(db.ServicesTranslateds.ToList());
        }

        public ActionResult ChangeAddedService(int id=0)
        {
            CheckConnection();
            if (db.ServicesTranslateds.Any(x => x.TourID == id))
            {
                ViewBag.Tour = db.Services.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.ToursEng = db.ServicesTranslateds.Where(x => x.TourID == id && x.LangCode == "en-US").FirstOrDefault();
                ViewBag.ToursRus = db.ServicesTranslateds.Where(x => x.TourID == id && x.LangCode == "ru-RU").FirstOrDefault();
                ViewBag.ToursGeo = db.ServicesTranslateds.Where(x => x.TourID == id && x.LangCode == "ka-GE").FirstOrDefault();
                ViewBag.Slider = db.ServicesPhotoes.Where(x => x.TourID == id).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("ChnageService");
            }
        }
        [HttpPost]
        public ActionResult ChangeAddedServices(TourModel model, int Id, HttpPostedFileBase Photo)
        {
            CheckConnection();
            var Eng = db.ServicesTranslateds.Where(x => x.TourID == Id && x.LangCode == "en-US").FirstOrDefault();
            var Rus = db.ServicesTranslateds.Where(x => x.TourID == Id && x.LangCode == "ru-RU").FirstOrDefault();
            var Geo = db.ServicesTranslateds.Where(x => x.TourID == Id && x.LangCode == "ka-GE").FirstOrDefault();
            Eng.Service.TourDate = model.TourDate;
            Eng.Service.Price = model.Price;
            if (Photo != null)
            {
                string name = MainHelper.Random32();
                string Pname = Eng.Service.Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                Eng.Service.Photo = "/images/Services/" + name + ".jpeg";
                using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 1000, 1000))
                {

                    string path = Server.MapPath("/images/Services/" + name + ".jpeg");

                    newimage.Save(path, ImageFormat.Jpeg);

                }

            }
            Eng.CategoryName = model.CategoryNameEng;
            Eng.Title = model.TitleEng;
            Eng.Text = model.TextEng;
            Eng.BigText = model.BigTextEng;
            db.SaveChanges();
            Rus.CategoryName = model.CategoryNameRus;
            Rus.Title = model.TitleRus;
            Rus.Text = model.TextRus;
            Rus.BigText = model.BigTextRus;
            db.SaveChanges();
            Geo.CategoryName = model.CategoryNameGeo;
            Geo.Title = model.TitleGeo;
            Geo.Text = model.TextGeo;
            Geo.BigText = model.BigTextGeo;
            db.SaveChanges();

            TempData["Success"] = "წარმატებით შეიცვალა";
            return RedirectToAction("ChnageService");
        }
        
        public ActionResult AddServicePhoto(IEnumerable<HttpPostedFileBase> Photo, int Id)
        {
            CheckConnection();
            foreach (var item in Photo)
            {
                if (item != null)
                {
                    string name = MainHelper.Random32();
                    db.ServicesPhotoes.Add(new ServicesPhoto()
                    {
                        TourID = Id,
                        Photo = "/images/ServiceSlider/" + name + ".jpeg",
                    });
                    using (var newimage = ScaleImage(Image.FromStream(item.InputStream, true, true), 1000, 1000))
                    {

                        string path = Server.MapPath("/images/ServiceSlider/" + name + ".jpeg");

                        newimage.Save(path, ImageFormat.Jpeg);

                    }
                    db.SaveChanges();

                }
            }
            TempData["Success"] = "წარმატებით დაემატა სლაიდერის ფოტოები";
            return RedirectToAction("ChangeAddedService", new { id = Id });

        }

        public ActionResult DelServSliderPhoto(int id)
        {
            CheckConnection();
            if (db.ServicesPhotoes.Any(x => x.Id == id))
            {
                var ID = db.ServicesPhotoes.Where(x => x.Id == id).FirstOrDefault().TourID;
                string Pname = db.ServicesPhotoes.Where(x => x.Id == id).FirstOrDefault().Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                db.ServicesPhotoes.Remove(db.ServicesPhotoes.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();
                TempData["Success"] = "წარმატებით წაიშალა";
                return RedirectToAction("ChangeAddedService", new { id = ID });

            }
            else
            {
                return RedirectToAction("ChnageService");
            }
        }

        public ActionResult DeleteService(int id)
        {
            CheckConnection();
            if (db.Services.Any(x => x.Id == id))
            {
                foreach (var item in db.ServicesPhotoes.Where(x => x.TourID == id))
                {
                    string name = item.Photo;
                    string fullpath = Request.MapPath(name);
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                }
                string cover = db.Services.Where(x => x.Id == id).FirstOrDefault().Photo;
                string full = Request.MapPath(cover);
                if (System.IO.File.Exists(full))
                {
                    System.IO.File.Delete(full);
                }
                db.ServicesPhotoes.RemoveRange(db.ServicesPhotoes.Where(x => x.TourID == id));
                db.ServicesTranslateds.RemoveRange(db.ServicesTranslateds.Where(x => x.TourID == id));
                db.Services.Remove(db.Services.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();

                TempData["Success"] = "წარმატებით წაიშალა";
                return RedirectToAction("ChnageService");
            }
            else
            {
                return RedirectToAction("ChnageService");
            }
        }

        //გალერეა

        public ActionResult Gallery()
        {
            CheckConnection();
            return View(db.Galleries.ToList());
        }

        public ActionResult AddGalleryPhoto(IEnumerable<HttpPostedFileBase> Photo)
        {
            CheckConnection();
            foreach(var item in Photo)
            {
                if (item != null)
                {
                    string name = MainHelper.Random32();
                    db.Galleries.Add(new Models.Gallery()
                    {
                        Photo= "/images/Gallery/"+name+".jpeg"
                    });
                    using (var newimage = ScaleImage(Image.FromStream(item.InputStream, true, true), 1000, 1000))
                    {

                        string path = Server.MapPath("/images/Gallery/" + name + ".jpeg");

                        newimage.Save(path, ImageFormat.Jpeg);

                    }
                    db.SaveChanges();

                }
            }
            TempData["Success"] = "წარმატებით აიტვირთა ფოტოები";
            return RedirectToAction("Gallery");
        }

        public ActionResult DelGalleryPhoto(int id)
        {
            CheckConnection();
            string Pname = db.Galleries.Where(x => x.Id == id).FirstOrDefault().Photo;
            string fullpath = Request.MapPath(Pname);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }
            db.Galleries.Remove(db.Galleries.Where(x => x.Id == id).FirstOrDefault());
            db.SaveChanges();
            return RedirectToAction("Gallery");
        }


        public ActionResult MainPage()
        {
            CheckConnection();

            return View(db.Sliders.ToList());
        }

        public ActionResult AddSlider(HttpPostedFileBase Photo,TourModel model)
        {
            CheckConnection();
            if(Photo != null)
            {
                string name = MainHelper.Random32();
                db.Sliders.Add(new Slider()
                {
                    
                    Photo = "/images/Slider/"+name+".jpeg"
                     
                });
                using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 2000, 2000))
                {

                    string path = Server.MapPath("/images/Slider/" + name + ".jpeg");

                    newimage.Save(path, ImageFormat.Jpeg);

                }
                db.SaveChanges();
                int id = db.Sliders.Max(x => x.Id);
                db.SliderTranslateds.Add(new SliderTranslated()
                {
                     SliderID=id,
                     LangCode=model.LangEng,
                     Text=model.TextEng,
                });
                db.SliderTranslateds.Add(new SliderTranslated()
                {
                    SliderID = id,
                    LangCode = model.LangRus,
                    Text = model.TextRus,
                });
                db.SliderTranslateds.Add(new SliderTranslated()
                {
                    SliderID = id,
                    LangCode = model.LangGeo,
                    Text = model.TextGeo,
                });
                db.SaveChanges();

            }
            TempData["Success"] = "წარმატებით დაემატა";
            return RedirectToAction("MainPage");
        }

        public ActionResult ChangeSliderText(TourModel model,HttpPostedFileBase Photo,int id)
        {
            CheckConnection();
            if(Photo != null)
            {
                string Pname = db.Sliders.Where(x => x.Id == id).FirstOrDefault().Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                var NewPhoto = db.Sliders.Where(x => x.Id == id).FirstOrDefault();
                string name = MainHelper.Random32();
                NewPhoto.Photo = "/images/Slider/" + name + ".jpeg";
                using (var newimage = ScaleImage(Image.FromStream(Photo.InputStream, true, true), 2000, 2000))
                {

                    string path = Server.MapPath("/images/Slider/" + name + ".jpeg");

                    newimage.Save(path, ImageFormat.Jpeg);

                }
                db.SaveChanges();

            }
            var eng = db.SliderTranslateds.Where(x => x.SliderID == id && x.LangCode == "en-US").FirstOrDefault();
            eng.Text = model.TextEng;
            var rus= db.SliderTranslateds.Where(x => x.SliderID == id && x.LangCode == "ru-RU").FirstOrDefault();
            rus.Text = model.TextRus;
            var geo = db.SliderTranslateds.Where(x => x.SliderID == id && x.LangCode == "ka-GE").FirstOrDefault();
            geo.Text = model.TextGeo;
            db.SaveChanges();
            TempData["Success"] = "წარმატებით შეიცვალა";
            return RedirectToAction("MainPage");
        }

        public ActionResult DelSlider(int id)
        {
            CheckConnection();
            if (db.Sliders.Any(x => x.Id == id))
            {
                string Pname = db.Sliders.Where(x => x.Id == id).FirstOrDefault().Photo;
                string fullpath = Request.MapPath(Pname);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                db.SliderTranslateds.RemoveRange(db.SliderTranslateds.Where(x => x.SliderID == id));
                db.Sliders.Remove(db.Sliders.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();
                TempData["Success"] = "წარმატებით წაიშალა";
            }
            return RedirectToAction("MainPage");
        }


        public ActionResult ChangeCategory()
        {
            CheckConnection();
            ViewBag.CategoryService = db.CategoryServiceTranslateds.ToList();
            return View(db.CategoryTranslateds.ToList());
        }

        public ActionResult ChangeAddedCategory(int id)
        {
            CheckConnection();
            ViewBag.ID = id;
            ViewBag.CategoryEng = db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "en-US").FirstOrDefault().Name;
            ViewBag.CategoryRus = db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ru-RU").FirstOrDefault().Name;
            ViewBag.CategoryGeo = db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ka-GE").FirstOrDefault().Name;

            return View();
        }

       [HttpPost]
        public ActionResult ChangeAddedCategory(int id,string LangEng,string LangRus,string LangGeo)
        {
            CheckConnection();
            db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "en-US").FirstOrDefault().Name = LangEng;
            db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ru-RU").FirstOrDefault().Name = LangRus;
            db.CategoryTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ka-GE").FirstOrDefault().Name = LangGeo;
            db.SaveChanges();
            TempData["Success"] = "წარმატებით შეიცვალა";
            return RedirectToAction("ChangeCategory");
        }

        public ActionResult DeleteCategory(int id)
        {
            CheckConnection();
            db.CategoryTranslateds.RemoveRange(db.CategoryTranslateds.Where(x => x.CategoryID == id));
            db.Categories.Remove(db.Categories.Where(x => x.Id == id).FirstOrDefault());
            db.SaveChanges();
            TempData["Success"] = "წარმატებით წაიშალა";
            return RedirectToAction("ChangeCategory");
        }

        public ActionResult ChangeAddedCatServ(int id)
        {
            CheckConnection();
            ViewBag.ID = id;
            ViewBag.CategoryEng = db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "en-US").FirstOrDefault().Name;
            ViewBag.CategoryRus = db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ru-RU").FirstOrDefault().Name;
            ViewBag.CategoryGeo = db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ka-GE").FirstOrDefault().Name;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeAddedCatServ(int id, string LangEng, string LangRus, string LangGeo)
        {
            CheckConnection();
            db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "en-US").FirstOrDefault().Name = LangEng;
            db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ru-RU").FirstOrDefault().Name = LangRus;
            db.CategoryServiceTranslateds.Where(x => x.CategoryID == id && x.LangCode == "ka-GE").FirstOrDefault().Name = LangGeo;
            db.SaveChanges();
            TempData["Success"] = "წარმატებით შეიცვალა";

            return RedirectToAction("ChangeCategory");
        }

        public ActionResult DeleteCatServ(int id)
        {
            CheckConnection();
            db.CategoryServiceTranslateds.RemoveRange(db.CategoryServiceTranslateds.Where(x => x.CategoryID == id));
            db.CategoryServices.Remove(db.CategoryServices.Where(x => x.Id == id).FirstOrDefault());
            db.SaveChanges();
            TempData["Success"] = "წარმატებით წაიშალა";

            return RedirectToAction("ChangeCategory");
        }








        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}