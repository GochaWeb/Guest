using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Models
{
    public class TourModel
    {
        //Eng
        [Required(ErrorMessage = "შეიყვანეთ ტურის თარიღი !")]
        [StringLength(50, ErrorMessage = "მაქსიმუმ, 50")]
        [AllowHtml]
        public string TourDate { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის ფასი !")]
        [StringLength(50, ErrorMessage = "მაქსიმუმ, 50")]
        [AllowHtml]
        public string Price { get; set; }

        [AllowHtml]
        public string by { get; set; }
        [AllowHtml]
        [DefaultValue(0)]
        public int Id { get; set; }

        //English
        public string LangEng { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია !")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        public string CategoryNameEng { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის სათაური ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string TitleEng { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის გარე ტექსტი ყველა ენაზე!")]
        [StringLength(500, ErrorMessage = "მაქსიმუმ, 500")]
        [AllowHtml]
        public string TextEng { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის შიდა ტექსტი ყველა ენაზე!")]
        [StringLength(4000, ErrorMessage = "მაქსიმუმ, 4000")]
        [AllowHtml]
        public string BigTextEng { get; set; }

        //Russian
        public string LangRus { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია !")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        public string CategoryNameRus { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის სათაური ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string TitleRus { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის გარე ტექსტი ყველა ენაზე!")]
        [StringLength(500, ErrorMessage = "მაქსიმუმ, 500")]
        [AllowHtml]
        public string TextRus { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის შიდა ტექსტი ყველა ენაზე!")]
        [StringLength(4000, ErrorMessage = "მაქსიმუმ, 4000")]
        [AllowHtml]
        public string BigTextRus { get; set; }

        //Georgian
        public string LangGeo { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია !")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        public string CategoryNameGeo { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის სათაური ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string TitleGeo { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის გარე ტექსტი ყველა ენაზე!")]
        [StringLength(500, ErrorMessage = "მაქსიმუმ, 500")]
        [AllowHtml]
        public string TextGeo { get; set; }
        [Required(ErrorMessage = "შეიყვანეთ ტურის შიდა ტექსტი ყველა ენაზე!")]
        [StringLength(4000, ErrorMessage = "მაქსიმუმ, 4000")]
        [AllowHtml]
        public string BigTextGeo { get; set; }

    }
}