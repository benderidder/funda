using BdR.Funda.ProgrammingAssignment;
using BdR.Funda.ProgrammingAssignment.Model;

Console.WriteLine("===============================================");
Console.WriteLine("BdR Funda Programming Assignment app started...");
Console.WriteLine("===============================================");

string city;
string? filter;

try
{
    //
    // There are 2 ways of using this console app...
    //
    if (args.Length > 0)
    {
        //
        // 1st: supply commanline arguments
        //
        city = args[0];
        filter = args.Length > 1 ? args[1] : null;

        if (city == string.Empty)
        {
            throw new Exception("No valid value for city was found in the command line arguments");
        }

        Console.WriteLine($"Found following arguments: city='{city}' filter='{filter ?? "none"}'");
    } 
    else
    {
        //
        // 2nd: enter requested search criteria
        //
        var defaultCity = "Amsterdam";

        // request for city to search in 
        Console.WriteLine($"Choose city for which you want the report (default = {defaultCity})");
        city = Console.ReadLine() ?? string.Empty;
        if (city == string.Empty) { city = defaultCity; }

        // request for additional search criterium
        Console.WriteLine("Choose filter criterium for kind of objects beeing searched (e.g. tuin, press enter for no additional search criteria)");
        filter = Console.ReadLine() ?? string.Empty;
        if (filter == string.Empty) { filter = null; }
    }

    // Ask the reporting service the desired report result...
    List<RealEstateAgent> realEstateAgents = ReportingService.Instance.GetReportTop10RealEstateAgent(city, filter);

    // Write table header
    Console.WriteLine();
    Console.WriteLine($"{"MAKELAAR".PadRight(60)}AANTAL_OBJECTEN");
    Console.WriteLine($"{"--------".PadRight(60)}---------------");

    // Write table body
    foreach (var a in realEstateAgents)
    {
        Console.WriteLine($"{a.Name.PadRight(60)}{a.ObjectCount}");
    }

    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error occurred: {ex.Message}");
}

Console.WriteLine("===============================================");
Console.WriteLine("BdR Funda Programming Assignment app finished!!");
Console.WriteLine("===============================================");

Console.Write("Press any key to close the app...");
Console.ReadKey();