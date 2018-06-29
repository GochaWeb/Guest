using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string NameGeo { get; set; }

        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string NameEng { get; set; }

        [Required(ErrorMessage = "შეიყვანეთ ტურის კატეგორია ყველა ენაზე!")]
        [StringLength(100, ErrorMessage = "მაქსიმუმ, 100")]
        [AllowHtml]
        public string NameRus { get; set; }

        public string LangGeo { get; set; }
        public string LangRus { get; set; }
        public string LangEng { get; set; }

    }
}