using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdR.Funda.ProgrammingAssignment.Model
{
    internal class PurchaseObjectCollection
    {
        public string Titel { get; set; } = string.Empty;

        public List<Object> Objects { get; set; } = new List<Object>();

        public Paging Paging { get; set; } = new Paging();
    }
}
