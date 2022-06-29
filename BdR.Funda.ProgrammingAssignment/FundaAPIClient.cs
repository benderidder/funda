using BdR.Funda.ProgrammingAssignment.Model;
using System.Text.Json;

namespace BdR.Funda.ProgrammingAssignment
{
    /// <summary>
    /// This client will interact with the Funda API on behalf of its caller
    /// </summary>
    internal class FundaAPIClient
    {
        private const string API_KEY = "ac1b0b1572524640a0ecc54de453ea9f";
        private const string BASE_URI = "http://partnerapi.funda.nl/feeds";

        /// <summary>
        /// Get purchaseble objects from API using the given arguments
        /// </summary>
        /// <param name="city">City to filter on, e.g. Amsterdam</param>
        /// <param name="filter">Optional addition filter argument (like 'tuin' = garden)</param>
        /// <returns>List of purchaseble objects meeting the filter requirements</returns>
        /// <exception cref="Exception">Any fatal request of unusable exception leads to an exception</exception>
        public List<Model.Object> GetPurchaseObjects(string city, string? filter = null)
        {
            PurchaseObjectCollection purchaseObjectCollection = new PurchaseObjectCollection();

            HttpClient client = new HttpClient();

            // some unsuccesful responses are by design, retries should be attempted
            int retryCounter = 0;

            // for each page of the total of pages, do the following...
            while(purchaseObjectCollection.Paging.HuidigePagina < purchaseObjectCollection.Paging.AantalPaginas)
            {
                string uri = $"{BASE_URI}/Aanbod.svc/json/{API_KEY}/?type=koop&zo=/{city.ToLower()}{(filter != null ? $"/{filter}" : "")}/&page={purchaseObjectCollection.Paging.HuidigePagina + 1}&pagesize=25";

                // get the actual response by doing a GET on the http client
                var response = client.GetAsync(uri).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (jsonString != string.Empty)
                    {
                        // convert json to POCO structure
                        var poc = JsonSerializer.Deserialize<PurchaseObjectCollection>(jsonString);

                        if (poc != null)
                        {
                            if (purchaseObjectCollection.Paging.HuidigePagina == 0)
                            {
                                // first time we set some base properties to our main container object...
                                purchaseObjectCollection = poc;
                            }
                            else
                            {
                                // after that we update paging and add the newly retrieved objects
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
                        // we try at most six times and wait for 10 sec, so 1 minute should have passed before giving up
                        Console.WriteLine("Unauthorize exception, maybe too many requests in a short time, waiting for some time and will try again...");
                        Thread.Sleep(10000);
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

            return purchaseObjectCollection.Objects;
        }
    }
}
