using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdR.Funda.ProgrammingAssignment.Model
{
    internal class Paging
    {
        public int AantalPaginas { get; set; } = 1;

        public int HuidigePagina { get; set; } = 0;

        public string VolgendeUrl { get; set; } = string.Empty;

        public string HuidigeUrl { get; set; } = string.Empty;
    }
}
