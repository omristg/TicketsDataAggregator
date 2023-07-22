using TicketsDataAggregator.FileAccess;
using TicketsDataAggregator.TicketsAggregation;

var ticketsFolder = "Tickets";

var test = "abc";
Console.WriteLine($"{test,-5}||");

try
{
    var app = new TicketsAggregator(ticketsFolder, new FileWriter(),new DocumentsPDFReader());
    app.Run();
}
catch (Exception ex)
{
    throw new Exception("Something error happened, program will be shut down", ex);
}


Console.ReadKey();
