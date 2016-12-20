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
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult List(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                //Get articles from the database
                var articles = database.Tags
                    .Include(t => t.Articles.Select(a => a.Tags))
                    .Include(t => t.Articles.Select(a => a.Author))
                    .FirstOrDefault(t => t.Id == id)
                    .Articles
                    .ToList();

                //Return the view
                return View(articles);
            }
            
        }

        //
        //GET: TagSearch
        public ActionResult TagSearch()
        {
            return View();
        }

        //
        //POST: TagSearch
        [HttpPost]
        public ActionResult TagSearch(TagSearch model)
        {
            if (ModelState.IsValid)
            {
                var database = new BlogDbContext();

                var tags = database.Tags.ToList();

                bool found = false;

                int tagID = -1;

                foreach(var tag in tags)
                {
                    if (tag.Name == model.keyWordSearch.ToLower())
                    {
                        found = true;
                        tagID = tag.Id;
                    }
                }

                if (found == true && tagID >= 0)
                {
                    return RedirectToAction("List", new { id = tagID });
                }
                else
                {
                    ModelState.AddModelError("", "Nothing found!");
                    return View(model);
                }
            }

            return View(model);

        }

    }
}