using GuestTours.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Controllers
{
    
    public class ToursController : BaseController
    {
        GuestToursEntities db;
        void CheckConnection()
        {
            if (db == null)
            {
                db = new GuestToursEntities();
            }
        }
        [ValidateInput(false)]
        public ActionResult View(TourModel tourmodel)
        {
            CheckConnection();
            
            if (db.Languages.Any(x => x.LangCode == Language))
            {

                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();
                if (tourmodel.by !=null)
                {
                   
                    var model = db.TourTranslateds.Where(x => x.CategoryName == tourmodel.by).ToList();
                    return View(model);
                }

                return View(db.TourTranslateds.Where(x => x.LangCode == Language).ToList());
            }
            else
            {
                if(tourmodel.by != null)
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    var model = db.TourTranslateds.Where(x => x.CategoryName == tourmodel.by && x.LangCode=="ka-GE").ToList();
                    return View(model);
                }
                ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                return View(db.TourTranslateds.Where(x => x.LangCode == "ka-GE").ToList());
            }
            
        }
        [ValidateInput(false)]
        public ActionResult Details(TourModel tourmodel)
        {
            CheckConnection();

            if(db.TourTranslateds.Any(x=>x.TourID== tourmodel.Id))
            {
                if (db.Languages.Any(x => x.LangCode == Language))
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == Language).ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == Language).ToList();

                    var model = db.TourTranslateds.Where(x => x.TourID == tourmodel.Id && x.LangCode == Language).FirstOrDefault();
                    ViewBag.Active = db.TourPhotoes.Where(x => x.TourID == tourmodel.Id).FirstOrDefault().Photo;
                    ViewBag.Slider = db.TourPhotoes.Where(x => x.TourID == tourmodel.Id).ToList();

                    ViewBag.Model = model;
                    return View();

                }
                else
                {
                    ViewBag.Category = db.CategoryTranslateds.Where(x => x.LangCode == "ka-GE").ToList();
                    ViewBag.CategoryService = db.CategoryServiceTranslateds.Where(x => x.LangCode == "ka-GE").ToList();

                    var model = db.TourTranslateds.Where(x => x.TourID == tourmodel.Id && x.LangCode == "ka-GE").FirstOrDefault();
                    ViewBag.Active = db.TourPhotoes.Where(x => x.TourID == tourmodel.Id).FirstOrDefault().Photo;
                    ViewBag.Slider = db.TourPhotoes.Where(x => x.TourID == tourmodel.Id).ToList();

                    ViewBag.Model = model;
                    return View();
                }
          
            }
            else
            {
                return RedirectToAction("View");
            }

            
        }
        [ValidateInput(false)]
        public JsonResult Res(Contact model, int Id)
        {
            CheckConnection();
            if(db.Tours.Any(x=>x.Id==Id) && !string.IsNullOrWhiteSpace(model.Name) && !string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Number) && !string.IsNullOrWhiteSpace(model.Text))
            {
                var tour = db.Tours.Where(x => x.Id == Id).FirstOrDefault();
                string Title = tour.TourTranslateds.FirstOrDefault(x => x.TourID == Id && x.LangCode == "ka-GE").Title;
                string Price = tour.Price;
                string Date = tour.TourDate;
                string Photo = tour.Photo;

                string mess = "<br />" + "ტურის სახელი : " + Title + "<br />" + "ტურის თარიღი : " + Date + "<br />" + "ტურის ფასი : " + Price;


                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("guesttoursend@gmail.com");
                mail.To.Add(new MailAddress("guesttourhelp@gmail.com"));
                mail.Subject = "დაჯავშნა";

                mail.Body = "მომხმარებლის სახელი:" + model.Name + "<br />" + "მომხმარებლის ელ-ფოსტა :" + model.Email + "<br />" + "ტელეფონი : " + model.Number + "<br />" + "ტექსტი : " + model.Text+mess;

                mail.IsBodyHtml = true;

                string fullpath = Request.MapPath(Photo);
                if (System.IO.File.Exists(fullpath))
                {
                    string file = fullpath;
                    Attachment attach = new Attachment(file);


                    mail.Attachments.Add(attach);
                }




                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();

                credential.UserName = "guesttoursend@gmail.com";
                credential.Password = "guest2018";

                smtp.Credentials = credential;
                smtp.EnableSsl = true;
                smtp.Send(mail);



                return Json(true,JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false);
            }
           
        }

        public JsonResult Get(string Id,string url)
        {
            CheckConnection();
            if(!string.IsNullOrEmpty(Id) && Id != null)
            {
                int ID = db.CategoryTranslateds.Where(x => x.Name == Id).FirstOrDefault().CategoryID;
                string name = db.CategoryTranslateds.Where(x => x.CategoryID == ID && x.LangCode == url).FirstOrDefault().Name;
                return Json(name);
            }
            else
            {
                return Json(false);
            }
        }

      





    }
}