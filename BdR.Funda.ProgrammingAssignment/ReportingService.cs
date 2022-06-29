using BdR.Funda.ProgrammingAssignment.Model;

namespace BdR.Funda.ProgrammingAssignment
{
    /// <summary>
    /// This service acts as middle tier between the executing program and the Funda API Client,
    /// it will query the client for results, interpretes the results and returns them in a reportable state.
    /// </summary>
    internal class ReportingService
    {
        private static ReportingService? _reportingService;

        /// <summary>
        /// Get singleton instance of service
        /// </summary>
        public static ReportingService Instance { 
            get
            {
                if(_reportingService == null)
                {
                    _reportingService = new ReportingService(); 
                }
                return _reportingService;
            }
        }

        /// <summary>
        /// Get top 10 of real estate brokers with most objects for sale, eventually filterd by extra search argument
        /// </summary>
        /// <param name="city">City to search in, like Amsterdam</param>
        /// <param name="filter">Additional search filter, like has Tuin (= garden)</param>
        /// <returns>Top 10 of real estate agents, ordered by object count descending</returns>
        public List<RealEstateAgent> GetReportTop10RealEstateAgent(string city, string? filter = null)
        {
            List<RealEstateAgent> realEstateAgents = new List<RealEstateAgent>();

            Console.WriteLine($"Searching for top 10 real estate agents with most houses for sale in '{city}'{(filter != null ? $" with search criteria '{filter}'" : "")}");

            // delegate Funda API interaction to client
            FundaAPIClient client = new FundaAPIClient();

            // ask the client to do the query
            var purchaseObjects = client.GetPurchaseObjects(city, filter);

            if (purchaseObjects == null || purchaseObjects.Count == 0)
            {
                Console.WriteLine("No purchases found");
            }
            else
            {
                // build a list of real esate agents and count their objects
                foreach (var o in purchaseObjects)
                {
                    var realEstateAgent = realEstateAgents.Where(r => r.Name == o.MakelaarNaam).FirstOrDefault();

                    if (realEstateAgent == null)
                    {
                        realEstateAgent = new RealEstateAgent() { Name = o.MakelaarNaam };
                        realEstateAgents.Add(realEstateAgent);
                    }

                    realEstateAgent.ObjectCount++;
                }

                // the final result will be ordered descending on object count and only the top 10 is returned
                realEstateAgents = realEstateAgents.OrderByDescending(r => r.ObjectCount).Take(10).ToList();
            }

            return realEstateAgents;
        }
    }
}
