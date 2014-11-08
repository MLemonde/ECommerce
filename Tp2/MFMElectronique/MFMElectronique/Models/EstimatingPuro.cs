using MFMElectronique.EstimatingProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MFMElectronique.Models
{
    public class EstimatingPuro
    {
        private EstimatingProxy.EstimatingService CreateEstimatingService()
        {
            EstimatingProxy.EstimatingService service = new EstimatingProxy.EstimatingService();

            // Setup the credentials for basic authentication
            //TODO : Define the Production (or development) Key and Password
            service.Credentials = new NetworkCredential("d43ca2e274134898bad98950e65cf9b4", "fb+@9VF{");

            // Set the request's context values
            service.RequestContextValue = new EstimatingProxy.RequestContext();
            service.RequestContextValue.Version = "1.4";
            service.RequestContextValue.Language = EstimatingProxy.Language.en;
            service.RequestContextValue.GroupID = "";
            service.RequestContextValue.RequestReference = "RequestReference";
            service.RequestContextValue.UserToken = "Your User Token";
            return service;
        }

        public void CallGetFullEstimate()
        {
            EstimatingProxy.EstimatingService service = CreateEstimatingService();
            EstimatingProxy.GetFullEstimateRequestContainer request = new EstimatingProxy.GetFullEstimateRequestContainer();
            EstimatingProxy.GetFullEstimateResponseContainer response = new EstimatingProxy.GetFullEstimateResponseContainer();

            // Setup the request to perform a get full estimate
            request.Shipment = CreateSampleShipment();

            try
            {
                // Call the service
                response = service.GetFullEstimate(request);

                // Display the response
                Display(response.ResponseInformation);
                Display("ShipmentEstimates:", response.ShipmentEstimates);
                Display("ReturnShipmentEstimates:", response.ReturnShipmentEstimates);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" EXCEPTION: {0}", ex.Message);
            }
        }

        public void CallGetQuickEstimate()
        {
            EstimatingProxy.EstimatingService service = CreateEstimatingService();
            EstimatingProxy.GetQuickEstimateRequestContainer request = new EstimatingProxy.GetQuickEstimateRequestContainer();
            EstimatingProxy.GetQuickEstimateResponseContainer response = new EstimatingProxy.GetQuickEstimateResponseContainer();

            // Setup the request to perform a create shipment
            //TODO : Define the Billing account and the account that is registered with PWS
            request.BillingAccountNumber = "9999999999";
            //TODO : Populate the Origin Information
            request.SenderPostalCode = "J2S1H9";
            //TODO : Populate the Desination Information
            request.ReceiverAddress = new EstimatingProxy.ShortAddress();
            request.ReceiverAddress.City = "Matane";
            request.ReceiverAddress.Country = "CA";
            request.ReceiverAddress.Province = "QC";
            request.ReceiverAddress.PostalCode = "G4W1M5";
            request.PackageType = "ExpressEnvelope";
            request.TotalWeight = new EstimatingProxy.TotalWeight();
            request.TotalWeight.Value = 1;
            request.TotalWeight.WeightUnit = EstimatingProxy.WeightUnit.lb;

            try
            {
                // Call the service
                response = service.GetQuickEstimate(request);

                // Display the response
                Display(response.ResponseInformation);
                Display("ShipmentEstimates:", response.ShipmentEstimates);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" EXCEPTION: {0}", ex.Message);
            }
        }

        private EstimatingProxy.Shipment CreateSampleShipment()
        {
            EstimatingProxy.Shipment shipment = new EstimatingProxy.Shipment();


            //Set Indicators - Required for various fields when creating a shipment            
            shipment.InternationalInformation = new EstimatingProxy.InternationalInformation();
            shipment.InternationalInformation.CustomsInvoiceDocumentIndicatorSpecified = true;
            shipment.InternationalInformation.ImportExportTypeSpecified = true;

            shipment.InternationalInformation.ContentDetails = new EstimatingProxy.ContentDetail[1];
            shipment.InternationalInformation.ContentDetails[0] = new ContentDetail();
            shipment.InternationalInformation.ContentDetails[0].FDADocumentIndicatorSpecified = true;
            shipment.InternationalInformation.ContentDetails[0].FCCDocumentIndicatorSpecified = true;
            shipment.InternationalInformation.ContentDetails[0].NAFTADocumentIndicatorSpecified = true;
            shipment.InternationalInformation.ContentDetails[0].SenderIsProducerIndicatorSpecified = true;
            shipment.InternationalInformation.ContentDetails[0].TextileIndicatorSpecified = true;

            shipment.PackageInformation.DangerousGoodsDeclarationDocumentIndicatorSpecified = true;


            //TODO : Populate the additional Sender Information
            shipment.SenderInformation = new SenderInformation();
            shipment.SenderInformation.Address = SetSenderAddress();
            shipment.SenderInformation.TaxNumber = "123456";
            //TODO : Populate the additional Destination Information
            shipment.ReceiverInformation = new ReceiverInformation();
            shipment.ReceiverInformation.Address = SetReceiverAddress();
            shipment.ReceiverInformation.TaxNumber = "654321";


            //TODO : Shipment Date - Future Dated Shipments YYYY-MM-DD
            shipment.ShipmentDate = "YOUR_DATE_HERE";

            //TODO : Shipment Date - Future Dated Shipments YYYY-MM-DD
            //This example adds one day to the current date                                  
            /*DateTime dt = DateTime.Now.AddDays(1.00);
            shipment.ShipmentDate = dt.ToString("yyyy-MM-dd");
            */

            //TODO : Populate the Package Information
            shipment.PackageInformation = new EstimatingProxy.PackageInformation();
            shipment.PackageInformation.ServiceID = "PurolatorExpressEnvelope";
            shipment.PackageInformation.Description = "Description";
            shipment.PackageInformation.TotalWeight = new EstimatingProxy.TotalWeight();
            shipment.PackageInformation.TotalWeight.Value = 1;
            shipment.PackageInformation.TotalWeight.WeightUnit = EstimatingProxy.WeightUnit.lb;
            shipment.PackageInformation.TotalPieces = 1;

            shipment.PackageInformation.PiecesInformation = new EstimatingProxy.Piece[1];
            shipment.PackageInformation.PiecesInformation[0] = SetPiece();
            shipment.PackageInformation.DangerousGoodsDeclarationDocumentIndicator = false;

            //TODO : Define OptionsInformation
            //ResidentialSignatureDomestic
            //shipment.PackageInformation.OptionsInformation = SetSignatureOption();

            //OriginSignatureNotRequired
            shipment.PackageInformation.OptionsInformation = SetOriginSignatureNotRequiredOption();

            //TODO : Populate the Paymment Information
            shipment.PaymentInformation = new EstimatingProxy.PaymentInformation();
            shipment.PaymentInformation.PaymentType = EstimatingProxy.PaymentType.Sender;
            shipment.PaymentInformation.RegisteredAccountNumber = "9999999999";
            shipment.PaymentInformation.BillingAccountNumber = "9999999999";

            //TODO : Populate the Pickup Information
            shipment.PickupInformation = new EstimatingProxy.PickupInformation();
            shipment.PickupInformation.PickupType = EstimatingProxy.PickupType.DropOff;

            shipment.NotificationInformation = new EstimatingProxy.NotificationInformation();
            shipment.NotificationInformation.ConfirmationEmailAddress = "my.name@example.com";

            shipment.TrackingReferenceInformation = new EstimatingProxy.TrackingReferenceInformation();
            shipment.TrackingReferenceInformation.Reference1 = "Reference1";
            shipment.TrackingReferenceInformation.Reference2 = "Reference2";
            shipment.TrackingReferenceInformation.Reference3 = "Reference3";
            shipment.TrackingReferenceInformation.Reference4 = "Reference4";

            shipment.OtherInformation = null;

            return shipment;
        }

        private void Display(EstimatingProxy.ResponseInformation respInf)
        {
            if (respInf == null)
                return;

            int i = 0;
            if (respInf.Errors != null && respInf.Errors.Length > 0)
            {
                foreach (EstimatingProxy.Error error in respInf.Errors)
                {
                    i++;
                    Util.Print("Error", i);
                    Util.Push();
                    Util.Print("Error code", error.Code);
                    Util.Print("Error description", error.Description);
                    Util.Print("Additional Information", error.AdditionalInformation);
                    Util.Pop();
                }
            }

            i = 0;
            if (respInf.InformationalMessages != null && respInf.InformationalMessages.Length > 0)
            {
                foreach (EstimatingProxy.InformationalMessage msg in respInf.InformationalMessages)
                {
                    i++;
                    Util.Print("InformationalMessage", i);
                    Util.Push();
                    Util.Print("", msg.Code);
                    Util.Print("message", msg.Message);
                    Util.Pop();
                }
            }
        }


        private EstimatingProxy.Address SetSenderAddress()
        {
            EstimatingProxy.Address addr = new EstimatingProxy.Address();

            addr.Name = "PWS User";
            addr.Company = "Company";
            addr.Department = "Department";
            addr.StreetNumber = "5995";
            addr.StreetSuffix = "";
            addr.StreetName = "AVEBURY";
            addr.StreetType = "Road";
            addr.StreetDirection = "";
            addr.Suite = "123";
            addr.Floor = "4";
            addr.StreetAddress2 = "";
            addr.StreetAddress3 = "";
            addr.City = "Mississauga";
            addr.Province = "ON";
            addr.Country = "CA";
            addr.PostalCode = "your postal code";
            addr.PhoneNumber = SetPhoneNumber();
            addr.FaxNumber = SetPhoneNumber();

            return addr;
        }

        private EstimatingProxy.Address SetReceiverAddress()
        {
            EstimatingProxy.Address addr = new EstimatingProxy.Address();

            addr.Name = "Another PWS User";
            addr.Company = "Another Company";
            addr.Department = "Department";
            addr.StreetNumber = "5995";
            addr.StreetSuffix = "";
            addr.StreetName = "AVEBURY";
            addr.StreetType = "Road";
            addr.StreetDirection = "";
            addr.Suite = "567";
            addr.Floor = "9";
            addr.StreetAddress2 = "";
            addr.StreetAddress3 = "";
            addr.City = "Mississauga";
            addr.Province = "ON";
            addr.Country = "CA";
            addr.PostalCode = "L5R3T8";
            addr.PhoneNumber = SetPhoneNumber();
            addr.FaxNumber = SetPhoneNumber();

            return addr;
        }

        EstimatingProxy.PhoneNumber SetPhoneNumber()
        {
            EstimatingProxy.PhoneNumber ph = new EstimatingProxy.PhoneNumber();
            ph.CountryCode = "1";
            ph.AreaCode = "905";
            ph.Phone = "7128101";
            ph.Extension = "9999";
            return ph;
        }

        EstimatingProxy.Piece SetPiece()
        {
            EstimatingProxy.Piece piece = new EstimatingProxy.Piece();

            piece.Weight = new EstimatingProxy.Weight();
            piece.Weight.Value = 1;
            piece.Weight.WeightUnit = EstimatingProxy.WeightUnit.lb;

            piece.Length = SetDimension(0);
            piece.Width = SetDimension(0);
            piece.Height = SetDimension(0);

            return piece;
        }

        EstimatingProxy.Dimension SetDimension(int value)
        {
            EstimatingProxy.Dimension dim = new EstimatingProxy.Dimension();
            dim.Value = value;
            dim.DimensionUnit = EstimatingProxy.DimensionUnit.@in;
            return dim;
        }

        EstimatingProxy.OptionsInformation SetSignatureOption()
        {
            EstimatingProxy.OptionsInformation opt = new OptionsInformation();
            opt.Options = new EstimatingProxy.OptionIDValuePair[1];
            opt.Options[0] = new OptionIDValuePair();
            opt.Options[0].ID = "residentialsignaturedomestic";
            opt.Options[0].Value = "true";
            return opt;
        }

        EstimatingProxy.OptionsInformation SetOriginSignatureNotRequiredOption()
        {
            EstimatingProxy.OptionsInformation opt = new OptionsInformation();
            opt.Options = new EstimatingProxy.OptionIDValuePair[1];
            opt.Options[0] = new OptionIDValuePair();
            opt.Options[0].ID = "OriginSignatureNotRequired";
            opt.Options[0].Value = "true";
            return opt;
        }

        void Display(string text, EstimatingProxy.ShipmentEstimate[] estimates)
        {
            if (estimates != null && estimates.Length > 0)
            {
                Console.WriteLine(text);
                Util.Push();
                foreach (EstimatingProxy.ShipmentEstimate estimate in estimates)
                {
                    Display(estimate);
                }
                Util.Pop();
            }
            else
            {
                Util.Print(text, " not available");
            }
        }

        void Display(EstimatingProxy.ShipmentEstimate estimate)
        {
            Util.Print("Service ID", estimate.ServiceID);
            Util.Print("Shipment Date", estimate.ShipmentDate);
            Util.Print("Expected Delivery Date", estimate.ExpectedDeliveryDate);
            Util.Print("Base Price", estimate.BasePrice);
            Display(estimate.Surcharges);
            Display(estimate.Taxes);
            Display(estimate.OptionPrices);
            Util.Print("TotalPrice", estimate.TotalPrice);
        }

        void Display(EstimatingProxy.Surcharge[] charges)
        {
            if (charges != null && charges.Length > 0)
            {
                Console.WriteLine("Surcharges:");
                Util.Push();
                foreach (EstimatingProxy.Surcharge charge in charges)
                {
                    Display(charge);
                }
                Util.Pop();
            }
            else
            {
                Console.WriteLine("Surcharges not available");
            }
        }

        void Display(EstimatingProxy.Surcharge charge)
        {
            Util.Print("Amount", charge.Amount);
            Util.Print("Type", charge.Type);
            Util.Print("Description", charge.Description);
        }

        void Display(EstimatingProxy.Tax[] taxes)
        {
            if (taxes != null && taxes.Length > 0)
            {
                Console.WriteLine("Taxes:");
                Util.Push();
                foreach (EstimatingProxy.Tax tax in taxes)
                {
                    Display(tax);
                }
                Util.Pop();
            }
            else
            {
                Console.WriteLine("Taxes not available");
            }
        }

        void Display(EstimatingProxy.Tax tax)
        {
            Util.Print("Amount", tax.Amount);
            Util.Print("Type", tax.Type);
            Util.Print("Description", tax.Description);
        }

        void Display(EstimatingProxy.OptionPrice[] prices)
        {
            if (prices != null && prices.Length > 0)
            {
                Console.WriteLine("OptionPrices:");
                Util.Push();
                foreach (EstimatingProxy.OptionPrice price in prices)
                {
                    Display(price);
                }
                Util.Pop();
            }
            else
            {
                Util.Print("OptionPrices not available");
            }
        }

        void Display(EstimatingProxy.OptionPrice price)
        {
            Util.Print("ID", price.ID);
            Util.Print("Amount", price.Amount);
            Util.Print("Description", price.Description);
        }
    }
}