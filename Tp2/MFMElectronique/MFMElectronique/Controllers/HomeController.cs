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
    [AllowAnonymous]
    public class HomeController : Controller
    {
        ElectroniqueEntities storeDB = new ElectroniqueEntities();

        /// <summary>
        /// Page d'accueil du site...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Méthode permettante de sélectionner un nouveau language et de refraichir la page
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectLanguage(string language, string returnUrl)
        {
            HttpContext.Session["Culture"] = language;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult History()
        {
            var Orderlist = storeDB.Orders.Where(c => c.AspNetUsers.Email == User.Identity.Name);
            if (Orderlist.Count() != 0 && Orderlist != null)
                return View(Orderlist);
            else
                return RedirectToAction("Index");
        }
    }
}