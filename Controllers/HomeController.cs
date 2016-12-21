using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListCategories");
        }

        public ActionResult ListCategories()
        {
            using (var database = new BlogDbContext())
            {
                var categories = database.Categories
                    .Include(c => c.Articles)
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(categories);
            }
        }

        public ActionResult ListArticles(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var articles = database.Articles
                    .Where(a => a.CategoryId == categoryId)
                    .Include(a => a.Author)
                    .Include(a => a.Tags)
                    .Include(a => a.Comments)
                    .ToList();

                return View(articles);
            }
        }

        public ActionResult ListMpArticles()
        {
            Dictionary<Article, int> temporary = new Dictionary<Article, int>();

            using (var database = new BlogDbContext())
            {

                var articles = database.Articles
                    .Include(a => a.Author)
                    .Include(a => a.Tags)
                    .Include(a => a.Comments)
                    .ToList();
                foreach (var art in articles)
                {
                    temporary.Add(art, art.Visits);
                }

                temporary = temporary.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);

                articles.RemoveRange(0, articles.Count());

                foreach (KeyValuePair<Article, int> pair in temporary)
                {
                    articles.Add(pair.Key);
                }

                return View(articles);
            }
        }

        public ActionResult ListMcArticles()
        {
            Dictionary<Article, int> temporary = new Dictionary<Article, int>();

            using (var database = new BlogDbContext())
            {

                var articles = database.Articles
                    .Include(a => a.Author)
                    .Include(a => a.Tags)
                    .Include(a => a.Comments)
                    .ToList();
                foreach (var art in articles)
                {
                    temporary.Add(art, art.Comments.Count());
                }

                temporary = temporary.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);

                articles.RemoveRange(0, articles.Count());

                foreach (KeyValuePair<Article, int> pair in temporary)
                {
                    articles.Add(pair.Key);
                }

                return View("ListMpArticles", articles);
            }
        }

    }
}