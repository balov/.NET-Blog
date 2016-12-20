using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class TagSearch
    {
        [Display(Name = "Tag")]
        [Required]
        public string keyWordSearch { get; set; }
    }
}