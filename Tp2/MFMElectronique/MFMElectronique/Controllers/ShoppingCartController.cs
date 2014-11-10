using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MFMElectronique.ViewModels;
using MFMElectronique.Models;

namespace MFMElectronique.Controllers
{
    public class ShoppingCartController : Controller
    {

        ElectroniqueEntities storeDB = new ElectroniqueEntities();
        //
        // GET: /ShoppingCart/
        [Authorize]
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
                
            };


            //PUROLATOR ET POSTE CANADA
            EstimatingPuro puroClient = new EstimatingPuro();            
            AspNetUser aUser = storeDB.AspNetUsers.First(c => c.Email == User.Identity.Name);
            viewModel.Shipping = puroClient.CallGetQuickEstimate(aUser);
            
            
            // Return the view
            return View(viewModel);
        }

        //
        // GET /EmptyCard/
        public ActionResult EmptyCard()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id, string returnUrl)
        {
            
            // Retrieve the album from the database
            var addedProduct = storeDB.Products
            .Single(produit => produit.Id == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct);
            // Go back to the main store page for more shopping
            //return RedirectToAction("Index");
            return Redirect(returnUrl);
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string albumName = storeDB.Carts
            .Single(item => item.RecordID == id).Product.Name;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        public ActionResult DeleteCart(int id)
        {
            return RedirectToAction("EmptyCard");
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
