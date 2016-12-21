using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Comment
    {
        private ICollection<Article> articles;

        public Comment()
        {
            this.articles = new HashSet<Article>();
        }

        public string Author { get; set; }

        [Display(Name = "Date")]
        public DateTime dateCreated { get; set; }

        [Key]
        public int Id { get; set; }

        
        [Required]
        public string Content { get; set; }

        public virtual ICollection<Article> Articles
        {
            get { return this.articles; }
            set { this.articles = value; }
        }


    }
}