using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Web;

namespace MFMElectronique.Models
{
    public class GetShipmentPrice
    {
        
            static void Main(string[] args)
            {
                
                // Your username, password and customer number are imported from the following file
                // CPCWS_SOAP_Shipping_DotNet_Samples\SOAP\shipping\user.xml 
                var username = "Mathewl01";
                var password = "test1!";
                var mailedBy = "2004381";

                PosteCanadaRef.ShipmentPortTypeClient server = new PosteCanadaRef.ShipmentPortTypeClient("ShipmentPort", "https://ct.soa-gw.canadapost.ca/rs/soap/shipment/v6");
                server.ClientCredentials.UserName.UserName = username;
                server.ClientCredentials.UserName.Password = password;

                // Create Request
                PosteCanadaRef.getshipmentpricerequest request = new PosteCanadaRef.getshipmentpricerequest();
                request.locale = "EN";
                request.mailedby = mailedBy;
                request.shipmentid = "932091379418015924";

                // Disable timestamp on request
                BindingElementCollection elements = server.Endpoint.Binding.CreateBindingElements();
                elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
                server.Endpoint.Binding = new CustomBinding(elements);

                String responseAsString = "";

                try
                {
                    // Execute Request
                    server.Open();
                    PosteCanadaRef.getshipmentpriceresponse response = server.GetShipmentPrice(request);
                    // Retrieve values from response object
                    if (response.Item.GetType() == typeof(PosteCanadaRef.ShipmentPriceType))
                    {
                        PosteCanadaRef.ShipmentPriceType shipmentPrice = (PosteCanadaRef.ShipmentPriceType)response.Item;
                        responseAsString += "Service Code: " + shipmentPrice.servicecode + "\r\n";
                        responseAsString += "Due amount: " + shipmentPrice.dueamount + "\r\n";
                    }
                    else
                    {
                        PosteCanadaRef.messages msgs = (PosteCanadaRef.messages)response.Item;
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
                    Console.WriteLine(faultEx);
                }
                catch (Exception ex)
                {
                    // Misc Exception
                    responseAsString += "ERROR: " + ex.Message + "\r\n";
                }

                server.Close();

                Console.WriteLine(responseAsString);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

            }
        
    }
}