﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ArticleViewModel
    {
        public string Tags { get; set; }

        public string Comment { get; set; }

        public int Id { get; set; }

        public string AuthorName { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public string AuthorId { get; set; }
       
        public int CategoryId { get; set; }
        public ICollection<Article> Articles { get; set; }
        public List<Category> Categories { get; internal set; }
    }
}