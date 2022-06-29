using BdR.Funda.ProgrammingAssignment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdR.Funda.ProgrammingAssignment
{
    internal class ReportingService
    {
        private static ReportingService? _reportingService;

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

        public List<RealEstateAgent> GetReportTop10RealEstateAgent(string city, string? filter = null)
        {
            List<RealEstateAgent> realEstateAgents = new List<RealEstateAgent>();

            // TODO: write implementation
            Console.WriteLine($"Searching for top 10 real estate agents with most houses for sale in '{city}'{(filter != null ? $" with search criteria '{filter}'" : "")}");

            FundaAPIClient client = new FundaAPIClient();

            var purchaseObjectCollection = client.GetPurchaseObjectCollection(city, filter);

            if (purchaseObjectCollection == null || purchaseObjectCollection.Objects == null)
            {
                Console.WriteLine("No purchases found");
            }
            else
            {
                foreach (var o in purchaseObjectCollection.Objects)
                {
                    var realEstateAgent = realEstateAgents.Where(r => r.Name == o.MakelaarNaam).FirstOrDefault();

                    if (realEstateAgent == null)
                    {
                        realEstateAgent = new RealEstateAgent() { Name = o.MakelaarNaam };
                        realEstateAgents.Add(realEstateAgent);
                    }

                    realEstateAgent.ObjectCount++;
                }

                realEstateAgents = realEstateAgents.OrderByDescending(r => r.ObjectCount).Take(10).ToList();
            }

            return realEstateAgents;
        }
    }
}
