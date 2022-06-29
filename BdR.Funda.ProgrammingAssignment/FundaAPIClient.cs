using BdR.Funda.ProgrammingAssignment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BdR.Funda.ProgrammingAssignment
{
    internal class FundaAPIClient
    {
        private const string API_KEY = "ac1b0b1572524640a0ecc54de453ea9f";
        private const string BASE_URI = "http://partnerapi.funda.nl/feeds";

        public PurchaseObjectCollection GetPurchaseObjectCollection(string city, string? filter = null)
        {
            PurchaseObjectCollection purchaseObjectCollection = new PurchaseObjectCollection();

            HttpClient client = new HttpClient();

            int retryCounter = 0;

            while(purchaseObjectCollection.Paging.HuidigePagina < purchaseObjectCollection.Paging.AantalPaginas)
            {
                string uri = $"{BASE_URI}/Aanbod.svc/json/{API_KEY}/?type=koop&zo=/{city.ToLower()}{(filter != null ? $"/{filter}" : "")}/&page={purchaseObjectCollection.Paging.HuidigePagina + 1}&pagesize=25";

                var response = client.GetAsync(uri).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (jsonString != String.Empty)
                    {
                        var poc = JsonSerializer.Deserialize<PurchaseObjectCollection>(jsonString);

                        if (poc != null)
                        {
                            if (purchaseObjectCollection.Paging.HuidigePagina == 0)
                            {
                                purchaseObjectCollection = poc;
                            }
                            else
                            {
                                purchaseObjectCollection.Paging = poc.Paging;
                                purchaseObjectCollection.Objects.AddRange(poc.Objects);
                            }

                            Console.WriteLine($"Retrieved page {purchaseObjectCollection.Paging.HuidigePagina} of {purchaseObjectCollection.Paging.AantalPaginas}, current object count: {purchaseObjectCollection.Objects.Count} objects");
                        }
                        else
                        {
                            throw new Exception("Content response resulted in empty object after deserialization");
                        }
                    }
                    else
                    {
                        throw new Exception("No content was returned by API");
                    }

                    retryCounter = 0;
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if(retryCounter < 6)
                    {
                        Console.WriteLine("Unauthorize exception, maybe be to many requests in a short time, waiting for some time and will try again...");
                        System.Threading.Thread.Sleep(10000);
                        retryCounter ++;
                    }
                    else
                    {
                        throw new Exception($"Unauthorized exception for too many times");
                    }
                }
                else
                {
                    throw new Exception($"Response API not succesfull, code returned: {response.StatusCode}");
                }
            }

            return purchaseObjectCollection;
        }
    }
}
