namespace BdR.Funda.ProgrammingAssignment.Model
{
    /// <summary>
    /// Paging object is part of collection calls and is necessary to retrieve all objects from all pages
    /// </summary>
    internal class Paging
    {
        public int AantalPaginas { get; set; } = 1;

        public int HuidigePagina { get; set; } = 0;

        public string VolgendeUrl { get; set; } = string.Empty;

        public string HuidigeUrl { get; set; } = string.Empty;
    }
}
