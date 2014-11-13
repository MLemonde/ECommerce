using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MFMElectronique.Models;
using MFMElectronique.ViewModels;
using System.IO;

namespace MFMElectronique.Controllers
{

    /// <summary>
    /// Class ADMIN : l'administrateur peut gérer les produits ici.
    /// Auteur du controlleur : MA
    /// </summary>

    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private ElectroniqueEntities db = new ElectroniqueEntities();


        // GET: Admin
        /// <summary>
        /// Retourne l'index de l'admin...
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductBrand).Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: Admin/Details/5
        /// <summary>
        /// On ne va pas l'utiliser mais retourne les détails d'un produit.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
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

        // GET: Admin/Create
        /// <summary>
        /// Retourne la page pour créer un nouveau produit
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name");
            ViewBag.CategoryID = new SelectList(db.ProductCategories.OrderBy(x => x.Name), "Id", "Name");

            return View();
        }

        // POST: Admin/Create
        /// <summary>
        /// Créer le produit, si on ne réussi retourne message d'erreur
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ProductAdminViewModel product)
        {
            if (ModelState.IsValid)
            {
                CreateProduct(product);
                //db.Products.Add(product);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryID);
            return View(product);
        }
        /// <summary>
        /// Lors de la création du produit, la méthode pour uploader une photo est un peu longue,
        /// je l'ai donc séparée.
        /// Auteur : MA
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        private void CreateProduct(ProductAdminViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }


            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    CategoryID = model.CategoryID,
                    BrandID = model.BrandID,
                    DescriptionEN = model.DescriptionEN,
                    DescriptionFR = model.DescriptionFR,
                    discontinued = false
                };

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/Images/Products/";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(model.ImageUpload.FileName));
                    var imageUrl = Path.Combine(uploadDir, Path.GetFileName(model.ImageUpload.FileName));
                    model.ImageUpload.SaveAs(imagePath);
                    product.PictureURL = imageUrl;
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
        }

        // GET: Admin/Edit/5
        /// <summary>
        /// Retourne les composantes de la page pour updater un produit.
        /// Auteur : MA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
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

            var model = new ProductAdminViewModel
            {
                Name = product.Name,
                CategoryID = product.CategoryID,
                BrandID = product.BrandID,
                Price = product.Price,
                discontinued = product.discontinued,
                DescriptionEN = product.DescriptionEN,
                DescriptionFR = product.DescriptionFR
            };

            ViewBag.BrandID = new SelectList(db.ProductBrands, "Id", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryID);
            return View(model);
        }

        // POST: Admin/Edit/5
        /// <summary>
        /// Controleur pour updater le produit. (incluant le changement de photo)
        /// Auteur : MA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ProductAdminViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.ImageUpload != null)
            {
                if (model.ImageUpload != null || model.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(model.ImageUpload.ContentType))
                {
                    ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                }
            }

            if (ModelState.IsValid)
            {
                var product = db.Products.Find(model.Id);
                if (product == null)
                {
                    return new HttpNotFoundResult();
                }

                product.Name = model.Name;
                product.Price = model.Price;
                product.BrandID = model.BrandID;
                product.CategoryID = model.CategoryID;
                product.DescriptionFR = model.DescriptionFR;
                product.DescriptionEN = model.DescriptionEN;
                product.discontinued = model.discontinued;

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/uploads";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                    model.ImageUpload.SaveAs(imagePath);
                    product.PictureURL = imageUrl;
                }

                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Delete/5
        /// <summary>
        /// En temps normal, un produit ne devrait jamais être supprimée.
        /// Il devrait plutôt être mis comme discontinué. Pour l'instant je n'ai donc pas touché à cette méthode.
        /// Auteur : MA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
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

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
