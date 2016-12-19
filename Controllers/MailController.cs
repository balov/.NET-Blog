using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

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
        public ActionResult Contact(Mail c)
        {
            //Validate Google recaptcha 
            //Source http://www.dotnetawesome.com/2015/12/google-new-recaptcha-using-aspnet-mvc.html 
            var response = Request["g-recaptcha-response"];
            string secretKey = "6Lfduw4UAAAAAEby0ZlFVkPLf7DqfWQ8ErGCIqjk";
            var grClient = new WebClient();
            var result = grClient.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            //ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

            //Sending the email
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

               // if (c.Attachment != null && c.Attachment.ContentLength > 0)
               // {
               //     message.Attachments.Add(new Attachment(c.Attachment.InputStream, Path.GetFileName(c.Attachment.FileName)));
               // }


                if (ModelState.IsValid && status == true)
                {
                    try
                    {
                        client.Send(message);
                        ModelState.AddModelError("", "Your message has been sent!");
                    }
                    catch(Exception)
                    {
                        ModelState.AddModelError("", "A problem occured. Please try again later!");
                    }
                }
                else
                {
                    if (status == false)
                    {
                        ModelState.AddModelError("", "Google reCaptcha validation failed!");
                    }
                    else
                    {
                        ModelState.AddModelError("", "A problem occured. Please try again later!");
                    }
                }

                return View();
               
            }

        }
    }
}
