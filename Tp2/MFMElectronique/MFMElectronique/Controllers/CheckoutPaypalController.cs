using MFMElectronique.Controllers;
using MFMElectronique.Models;
//using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFMElectronique.Controllers
{
    public class CheckOutPaypalController : Controller
    {
        ElectroniqueEntities storeDB = new ElectroniqueEntities();
        string shippingAdress;
        
        /// <summary>
        /// Permet de transmettre une requête d'achat à Paypal.
        /// Une fois accepté, il sera redigiré vers la page de 
        /// confirmation d'achat
        /// </summary>
        /// <returns></returns>
        public ActionResult Checkout()
        {
            PaypalNVPAPICaller test = new PaypalNVPAPICaller();
            string retMsg = "";
            string token = "";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            decimal dMontantTotal = cart.GetTotal();
            decimal dshipping = 0;

            EstimatingPuro puroClient = new EstimatingPuro();
            AspNetUser aUser = storeDB.AspNetUsers.First(c => c.Email == User.Identity.Name);
            dshipping = puroClient.CallGetQuickEstimate(aUser);
            decimal Pcanada = new GetShipmentPrice().GetPrice(aUser.PostalCode);
            if (dshipping > Pcanada && Pcanada != 0)
            {
                dshipping = Pcanada;
            }

            if (dMontantTotal > 0)
            {
                decimal twoDec = Math.Round(dMontantTotal + dshipping, 2);
                string amt = twoDec.ToString().Replace(",", ".");
                Session["payment_amt"] = amt;
                bool ret = test.ShortcutExpressCheckout(amt, ref token, ref retMsg, cart.GetCartItems(),dshipping);
                if (ret)
                {
                    HttpContext.Session["token"] = token;
                    return Redirect(retMsg);
                }
                else
                {
                    return Redirect("APIError?" + retMsg);
                }
            }
            else
            {
                return Redirect("APIError?ErrorCode=AmtMissing");
            }
            
        }

        /// <summary>
        /// Cette méthode est appelé sr le retour de Paypal pour confirmer 
        /// la transaction et confirmer l'adresse du client
        /// </summary>
        /// <param name="token">Token de transaction</param>
        /// <param name="payerID">ID du Client de Paypal</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ConfirmPaiement(string token, string payerID)
        {
            PaypalNVPAPICaller test = new PaypalNVPAPICaller();

            string retMsg = "";

            bool ret = test.GetShippingDetails(token, ref payerID, ref shippingAdress, ref retMsg);
            if (ret)
            {
                Session["payerId"] = payerID;
                Session["token"] = token;
                Session["shipping"] = shippingAdress;
                ViewBag.shipping = shippingAdress;
            }
            else
            {
                return Redirect("APIError?" + retMsg);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ConfirmerTransaction(string shipping)
        {
            PaypalNVPAPICaller test = new PaypalNVPAPICaller();

            string retMsg = "";
            string token = "";
            string finalPaymentAmount = "";
            string payerId = "";
            NVPCodec decoder = null;

            token = Session["token"].ToString();
            payerId = Session["payerId"].ToString();
            finalPaymentAmount = Session["payment_amt"].ToString();

            bool ret = test.ConfirmPayment(finalPaymentAmount, token, payerId, ref decoder, ref retMsg);
            if (ret)
            {
                // Unique transaction ID of the payment. Note:  If the PaymentAction of the request was Authorization or Order, this value is your AuthorizationID for use with the Authorization & Capture APIs. 
                string transactionId = decoder["PAYMENTINFO_0_TRANSACTIONID"];

                // The type of transaction Possible values: l  cart l  express-checkout 
                string transactionType = decoder["PAYMENTINFO_0_TRANSACTIONTYPE"];

                // Indicates whether the payment is instant or delayed. Possible values: l  none l  echeck l  instant 
                string paymentType = decoder["PAYMENTINFO_0_PAYMENTTYPE"];

                // Time/date stamp of payment
                string orderTime = decoder["PAYMENTINFO_0_ORDERTIME"];

                // The final amount charged, including any shipping and taxes from your Merchant Profile.
                string amt = decoder["PAYMENTINFO_0_AMT"];

                // A three-character currency code for one of the currencies listed in PayPay-Supported Transactional Currencies. Default: USD.    
                string currencyCode = decoder["PAYMENTINFO_0_CURRENCYCODE"];

                // PayPal fee amount charged for the transaction    
                string feeAmt = decoder["PAYMENTINFO_0_FEEAMT"];

                // Amount deposited in your PayPal account after a currency conversion.    
                string settleAmt = decoder["PAYMENTINFO_0_SETTLEAMT"];

                // Tax charged on the transaction.    
                string taxAmt = decoder["PAYMENTINFO_0_TAXAMT"];

                //' Exchange rate if a currency conversion occurred. Relevant only if your are billing in their non-primary currency. If 
                string exchangeRate = decoder["PAYMENTINFO_0_EXCHANGERATE"];
                var cart = ShoppingCart.GetCart(this.HttpContext);


                //If it gets here, the order went all succesful, and it will now create the order in the database, in order to keep a history
                Order order = new Order();
                AspNetUser aUser = storeDB.AspNetUsers.First(c => c.Email == User.Identity.Name);
                order.AspNetUsers = aUser;
                string[] addr = Session["shipping"].ToString().Split(':','.');
                order.FirstName = aUser.FirstName;
                order.LastName = aUser.Lastname;
                order.City = aUser.City;
                order.Address = aUser.Address;
                order.State = aUser.State;
                order.PostalCode = aUser.PostalCode;
                order.OrderDate = DateTime.Now;
                order.Country = aUser.Country;
                order.Phone = aUser.PhoneNumber;
                string stotal = Session["payment_amt"].ToString();
                stotal = stotal.Replace('.', ',');
                float itotal = float.Parse(stotal);
                order.Total = (decimal)itotal;

                var lstitem = cart.GetCartItems();
                foreach(var i in lstitem)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.ProductId = i.ProductID;
                    detail.Quantity = i.Count;
                    detail.UnitPrice = i.TotalPerItem;
                    detail.Order = order;
                    order.OrderDetail.Add(detail);
                }


                storeDB.Orders.Add(order);
                cart.EmptyCart();
                storeDB.SaveChanges();





                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect("APIError?" + retMsg);
            }
        }
        
        [HttpGet]
        public ActionResult CancelPaiement(string token)
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Lorsqu'une erreur s'est produite avec Paypal
        /// </summary>
        /// <param name="ErrorCode"></param>
        /// <param name="Desc1"></param>
        /// <param name="Desc2"></param>
        /// <returns></returns>
        public ActionResult APIError(string ErrorCode, string Desc1, string Desc2)
        {
            ViewBag.Error = ErrorCode;
            ViewBag.Desc = Desc1;
            ViewBag.Desc2 = Desc2;
            return View();
        }
    }
}