using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MFMElectronique.Models;

namespace MFMElectronique.Controllers
{
    public class ProductsController : Controller
    {
        private ElectroniqueEntities db = new ElectroniqueEntities();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Products.Include(p => p.ProductBrand).Include(p => p.ProductCategory);
            return View(product.ToList());
        }

        /// <summary>
        /// GET : Liste des téléphones
        /// </summary>
        /// <returns></returns>
        public ActionResult Cellphone()
        {
            var product = db.Products.Include(p => p.ProductBrand).Include(p => p.ProductCategory);

            var phoneQuery = from d in product
                           where d.ProductCategory.Id == 1
                           select d;

            return View(phoneQuery.ToList());
        }

        /// <summary>
        /// GET : Liste des tablettes
        /// </summary>
        /// <returns></returns>
        public ActionResult Tablet()
        {
            var product = db.Products.Include(p => p.ProductBrand).Include(p => p.ProductCategory);

            var tabletQuery = from d in product
                             where d.ProductCategory.Id == 2
                             select d;

            return View(tabletQuery.ToList());
        }

        /// <summary>
        /// GET : Liste des montres
        /// </summary>
        /// <returns></returns>
        public ActionResult Watch()
        {
            var product = db.Products.Include(p => p.ProductBrand).Include(p => p.ProductCategory);

            var watchQuery = from d in product
                             where d.ProductCategory.Id == 3
                             select d;

            return View(watchQuery.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Permet de faire la recherche des produits à partir du textbox recherche.
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="productPrice"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult SearchIndex(string productCategory, string productPrice, string searchString)
        {

            var GenreLst = new List<string>();
            var GenreQry = from d in db.Products
                           orderby d.ProductCategory.Name
                           select d.ProductCategory.Name;
            GenreLst.AddRange(GenreQry.Distinct()); // Il reste à passer la liste en viewbag
            ViewBag.productCategory = new SelectList(GenreLst);

            //var PriceLst = new List<string>();
            //var PriceQry = from d in db.Products
            //               orderby d.ProductCategory.Id
            //               select d.ProductCategory.Name;
            //PriceLst.AddRange(PriceQry.Distinct());
            //ViewBag.productPrice = new SelectList(PriceLst);

            var products = from m in db.Products
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()) 
                    || s.ProductBrand.Name.ToUpper().Contains(searchString.ToUpper()));

            }


            if (String.IsNullOrEmpty(productCategory))
                return View(products);
            else
            {
                products = products.Where(x => x.ProductCategory.Name == productCategory);
                return View(products);
            }
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
