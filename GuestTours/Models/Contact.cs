using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestTours.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "Enter Your Name !")]
        [StringLength(100, ErrorMessage = "Max, 100 Symbol")]
        [AllowHtml]
        public string Name { get; set; }

       
        [Required(ErrorMessage = "Enter Your Email !")]
        [StringLength(500, ErrorMessage = "Max, 500 Symbol")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
  + "@"
  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Please, Enter Email !")]
        //[DataType(DataType.EmailAddress)]
        [AllowHtml]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your Phone Number !")]
        [StringLength(100, ErrorMessage = "Max, 100 Symbol")]
        [AllowHtml]
        public string Number { get; set; }

        [Required(ErrorMessage = "Enter Your Text!")]
        [StringLength(4000, ErrorMessage = "Max, 4000 Symbol")]
        [AllowHtml]
        public string Text { get; set; }

    }
}