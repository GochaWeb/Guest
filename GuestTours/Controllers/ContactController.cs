using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using GuestTours.Models;

namespace GuestTours.Controllers
{
    public class ContactController : BaseController
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
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Send(Contact model)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("guesttoursend@gmail.com");
            mail.To.Add(new MailAddress("guesttourhelp@gmail.com"));
            mail.Subject = "Support GuestTours";

            mail.Body = "მომხმარებლის სახელი:"+model.Name+ "<br />"+ "მომხმარებლის ელ-ფოსტა :" + model.Email + "<br />" + "ტექსტი : " + model.Text;

            mail.IsBodyHtml = true;





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

            TempData["Success"] = "მესიჯი წარმატებით გაიგზავნა";
            return RedirectToAction("Index");
        }
    }
}