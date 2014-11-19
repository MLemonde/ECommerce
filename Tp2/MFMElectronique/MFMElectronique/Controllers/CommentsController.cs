using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MFMElectronique.Models;
using System.Text;

namespace MFMElectronique.Controllers
{
    /// <summary>
    /// Controleur pour les commentaires et les ratings sur les produits.
    /// J'ai juste modifié la méthode create pour rajouter le return url.
    /// Auteur du controleur : MA
    /// </summary>
    public class CommentsController : Controller
    {
        private ElectroniqueEntities db = new ElectroniqueEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.AspNetUsers).Include(c => c.Product);
            return View(comments.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ProductID = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Comments/Create
        /// <summary>
        /// La création du commentaire
        /// </summary>
        /// <param name="comment">le commentaire</param>
        /// <param name="returnUrl">l'URL à laquelle on retourne après avoir créer le commentaire</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Throttle(Name = "TestThrottle", Message = "You must wait {n} seconds before posting a new comment.", Seconds = 60)]
        public ActionResult Create([Bind(Include = "Id,Rating,Comment1,DateCreated,ProductID,UserID")] Comment comment, string returnUrl)
        {
            comment.DateCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Here I will check is captcha code is valid or not
                bool isCaptchaCodeValid = false;
                string CaptchaMessage = "";
                isCaptchaCodeValid = GetCaptchaResponse(out CaptchaMessage);

                if (isCaptchaCodeValid)
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return Redirect(returnUrl);
                }
                else
                {
                    //ViewBag.Message = CaptchaMessage;
                    TempData["shortMessage"] = CaptchaMessage;
                    return Redirect(returnUrl);
                }
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", comment.UserID);
            ViewBag.ProductID = new SelectList(db.Products, "Id", "Name", comment.ProductID);
            return Redirect(returnUrl);
        }

        private bool GetCaptchaResponse(out string message)
        {
            bool flag = false;
            message = "";

            string[] result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com/recaptcha/api/verify");
            request.ProtocolVersion = HttpVersion.Version10;
            request.Timeout = 0x7530;
            request.Method = "POST";
            request.UserAgent = "reCAPTCHA/ASP.NET";
            request.ContentType = "application/x-www-form-urlencoded";
            string formData = string.Format(
                "privatekey={0}&remoteip={1}&challenge={2}&response={3}",
                new object[]{
            HttpUtility.UrlEncode("6Lfi-f0SAAAAAHKr_lioPACGncVmFVHFD5RS3uM6"),
            HttpUtility.UrlEncode(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()),
            HttpUtility.UrlEncode(Request.Form["recaptcha_challenge_field"]),
            HttpUtility.UrlEncode(Request.Form["recaptcha_response_field"])
        });
            byte[] formbytes = Encoding.ASCII.GetBytes(formData);

            using (System.IO.Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formbytes, 0, formbytes.Length);

            }

            try
            {
                using (WebResponse httpResponse = request.GetResponse())
                {
                    using (System.IO.TextReader readStream = new System.IO.StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd().Split(new string[] { "\n", @"\n" }, StringSplitOptions.RemoveEmptyEntries);
                        message = result[1];
                        flag = Convert.ToBoolean(result[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return flag;
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", comment.UserID);
            ViewBag.ProductID = new SelectList(db.Products, "Id", "Name", comment.ProductID);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rating,Comment1,DateCreated,ProductID,UserID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", comment.UserID);
            ViewBag.ProductID = new SelectList(db.Products, "Id", "Name", comment.ProductID);
            return View(comment);
        }

        [HttpGet]
        public ActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            Comment commentToDelete = db.Comments.Find(id);
            db.Comments.Remove(commentToDelete);
            db.SaveChanges();
            //return View(comment);
            return Redirect(returnUrl);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
