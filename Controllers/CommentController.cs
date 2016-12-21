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
    public class CommentController : Controller
    {
        //
        // GET: Article/Comment
        [Authorize]
        public ActionResult Comment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // Get article from the database
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .First();

                // Check if article exists
                if (article == null)
                {
                    return HttpNotFound();
                }

                // Create the view model
                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;
                model.AuthorName = article.Author.FullName;

                // Show the comment view
                return View(model);
            }
        }

        //
        // POST: Comment/Create
        [HttpPost]
        [Authorize]
        public ActionResult Comment(ArticleViewModel model)
        {

            using (var database = new BlogDbContext())
            {
                var commentContent = model.Comment;

                Article article = database.Articles.Where(a => a.Id == model.Id).First();

                var authorName = database.Users
                  .Where(u => u.UserName == this.User.Identity.Name)
                  .First()
                  .FullName;

                Comment comment = new Comment();
                comment.Content = commentContent;
                comment.dateCreated = DateTime.Now;
                comment.Author = authorName;

                //Check if the form is filled
                if (comment.Content == null)
                {
                    return RedirectToAction("Comment", new { id = article.Id });
                }
                //If yes save to the database
                else
                {
                    database.Comments.Add(comment);

                    article.Comments.Add(comment);

                    database.SaveChanges();

                    return RedirectToAction("Details", "Article", new { id = article.Id });
                }

            }
        }

        //
        // GET: Comment/Delete
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // Get article from database
                var comment = database.Comments
                  .Where(c => c.Id == id)
                  .Include(c => c.Articles)
                  .First();

                if (!User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // Check if comment exists
                if (comment == null)
                {
                    return HttpNotFound();
                }

                // Pass article to view
                return View(comment);
            }
        }

        //
        // POST: Comment/Delete
        [HttpPost, Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // Get comment from database
                var comment = database.Comments
                    .Where(c => c.Id == id)
                    .Include(c => c.Articles)
                    .First();

                int commentArticleId = comment.Articles.First().Id;

                // Check if comment exists
                if (comment == null)
                {
                    return HttpNotFound();
                }

                // Delete comment from database
                database.Comments.Remove(comment);
                database.SaveChanges();

                // Redirect to details page
                return RedirectToAction("Details", "Article", new { id = commentArticleId });
            }
        }

    }
}