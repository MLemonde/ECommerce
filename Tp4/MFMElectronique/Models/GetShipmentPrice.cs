using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Web;
using MFMElectronique.PosteCanadaRef;

namespace MFMElectronique.Models
{
    public class GetShipmentPrice
    {
        
            public decimal GetPrice(string PostalCode)
            {

                var username = "304c3d10815056ce";
                var password = "44987ecf42dce1cff0c066";
                var mailedBy = "0008307695";
                decimal dprix = 0;
                RatingPortTypeClient server = new RatingPortTypeClient("RatingPort", "https://ct.soa-gw.canadapost.ca/rs/soap/rating/v3");
                server.ClientCredentials.UserName.UserName = username;
                server.ClientCredentials.UserName.Password = password;

                // Create Request
                getratesrequest request = new getratesrequest();
                request.locale = "FR";

                request.mailingscenario = new getratesrequestMailingscenario();
                request.mailingscenario.parcelcharacteristics = new getratesrequestMailingscenarioParcelcharacteristics();
                request.mailingscenario.destination = new getratesrequestMailingscenarioDestination();
                getratesrequestMailingscenarioDestinationDomestic destDom = new getratesrequestMailingscenarioDestinationDomestic();
                request.mailingscenario.destination.Item = destDom;

                request.mailingscenario.customernumber = mailedBy;
                request.mailingscenario.parcelcharacteristics.weight = 1;
                request.mailingscenario.originpostalcode = "J2S0A1";
                destDom.postalcode = PostalCode;

                // Disable timestamp on request
                BindingElementCollection elements = server.Endpoint.Binding.CreateBindingElements();
                elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
                server.Endpoint.Binding = new CustomBinding(elements);

                String responseAsString = "";

                try
                {
                    // Execute Request
                    getratesresponse response = server.GetRates(request);

                    // Retrieve values from response object
                    if (response.Item.GetType() == typeof(getratesresponsePricequotes))
                    {
                        getratesresponsePricequotes priceQuotes = (getratesresponsePricequotes)response.Item;
                        foreach (var priceQuote in priceQuotes.pricequote)
                        {
                            responseAsString += "Service Name: " + priceQuote.servicename + "\r\n";
                            responseAsString += "Reception time: " + priceQuote.servicestandard.expecteddeliverydate + "\r\n";

                            responseAsString += "Price Name: $" + priceQuote.pricedetails.due + "\r\n\r\n";
                            if (dprix == 0 || dprix > priceQuote.pricedetails.due)
                                dprix = priceQuote.pricedetails.due;
                        }
                    }
                    else
                    {
                        messages msgs = (messages)response.Item;
                        foreach (var item in msgs.message)
                        {
                            responseAsString += "Error Code: " + item.code + "\r\n";
                            responseAsString += "Error Msg: " + item.description + "\r\n";
                        }
                    }
                }
                catch (FaultException faultEx)
                {
                    // SOAP Fault
                    responseAsString += "Fault Code: " + faultEx.Code.Name + "\r\n";
                    responseAsString += "Fault String: " + faultEx.Message + "\r\n";
                }
                catch (Exception ex)
                {
                    // Misc Exception
                    responseAsString += "ERROR: " + ex.Message + "\r\n";
                }

                Console.WriteLine(responseAsString);
                Console.WriteLine("Press any key to continue...");
                
                return dprix;

            }
        
    }
}