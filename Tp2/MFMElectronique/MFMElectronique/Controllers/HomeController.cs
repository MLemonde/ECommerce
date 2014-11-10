using MFMElectronique.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MFMElectronique.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Page d'accueil du site...
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Méthode permettante de sélectionner un nouveau language et de refraichir la page
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public ActionResult SelectLanguage(string language, string returnUrl)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    ElectroniqueEntities context = new ElectroniqueEntities();
            //    AspNetUser user = context.AspNetUsers.First(u => u.Email == User.Identity.Name);
            //    context.Dispose();
            //}
            HttpContext.Session["Culture"] = language;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}