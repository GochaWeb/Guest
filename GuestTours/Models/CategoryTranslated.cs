//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuestTours.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoryTranslated
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Language Language { get; set; }
    }
}