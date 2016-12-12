using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net;

namespace Blog.Controllers
{
    public class MailController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }


        //SEND: Contact form
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Send(Mail c)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587)) // netstat -l/a ... for checking available ports
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("alexandre.balov@gmail.com", "balov871016");


                string body = string.Format("Name: {0}\nEmail: {1}\nMessage: {2}",
                    c.Name,
                    c.Email,
                    c.Message
                );

                var message = new MailMessage();
                message.To.Add("alexandre.balov@gmail.com");
                message.From = new MailAddress(c.Email, c.Name);
                message.Subject = String.Format("Contact Request From: {0} ", c.Name);
                message.Body = body;
                message.IsBodyHtml = false;

                client.Send(message);

                return RedirectToAction("Index", "Home");

            }

        }
    }
}
