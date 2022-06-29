namespace BdR.Funda.ProgrammingAssignment.Model
{
    /// <summary>
    /// POCO that acts as main container for the json returned by the Funda API
    /// </summary>
    internal class PurchaseObjectCollection
    {
        public string Titel { get; set; } = string.Empty;

        public List<Object> Objects { get; set; } = new List<Object>();

        public Paging Paging { get; set; } = new Paging();
    }
}
