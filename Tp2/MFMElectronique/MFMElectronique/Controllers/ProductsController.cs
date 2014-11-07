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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name");
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DescriptionFR,DescriptionEN,discontinued,PictureURL,Price,CategoryID,BrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DescriptionFR,DescriptionEN,discontinued,PictureURL,Price,CategoryID,BrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                products = products.Where(s => s.Name.Contains(searchString) || s.ProductBrand.Name.Contains(searchString));

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
