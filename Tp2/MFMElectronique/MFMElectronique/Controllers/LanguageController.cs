using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFMElectronique.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public ActionResult SelectLanguage(string language)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SelectLanguage()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SelectLanguage(int language)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}