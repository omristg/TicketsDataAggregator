using System.Text;
using System.Globalization;
using TicketsDataAggregator.FileAccess;
using TicketsDataAggregator.Extensions;

namespace TicketsDataAggregator.TicketsAggregation;

public class TicketsAggregator
{
    private readonly IFileWriter _fileWriter;
    private readonly string _filesDirectory;
    private readonly IDocumentsReader _documentsReader;
    public static readonly Dictionary<string, CultureInfo> DomainToCultureMap = new()
    {
        [".com"] = new CultureInfo("en-US"),
        [".fr"] = new CultureInfo("fr-FR"),
        [".jp"] = new CultureInfo("jp-JP"),
    };
    public TicketsAggregator(string filesDirectory, IFileWriter fileWriter, IDocumentsReader documentsReader)
    {
        _filesDirectory = filesDirectory;
        _fileWriter = fileWriter;
        _documentsReader = documentsReader;
    }

    public void Run()
    {
        var stringBuilder = new StringBuilder();
        var documents = _documentsReader.Read(_filesDirectory);
        foreach (var document in documents)
        {
            var lines = ProccessDocument(document);
            stringBuilder.AppendLine(string.Join(Environment.NewLine, lines));

        }
        _fileWriter.Write(stringBuilder.ToString(), _filesDirectory, "AggregatedData.txt");

    }

    private static IEnumerable<string> ProccessDocument(string document)
    {
        var textualDataArray = document.Split(
                        new string[] { "Title:", "Date:", "Time:", "Visit us:" },
                        StringSplitOptions.None);

        var documentDomain = textualDataArray.Last().ExtractDomain();
        var culture = DomainToCultureMap[documentDomain];

        for (int i = 1; i < textualDataArray.Length - 3; i += 3)
        {
            var ticketString = BuildTicketData(textualDataArray, culture, i);
            yield return ticketString;
        }
    }

    private static string BuildTicketData(string[] textualDataArray, CultureInfo culture, int i)
    {
        string title = textualDataArray[i];
        string dateAsString = textualDataArray[i + 1];
        string timeAsString = textualDataArray[i + 2];

        var date = DateOnly.Parse(dateAsString, culture);
        var time = TimeOnly.Parse(timeAsString, culture);

        var invariantDate = date.ToString(CultureInfo.InvariantCulture);
        var invariantTime = time.ToString(CultureInfo.InvariantCulture);

        return $"{title,-40}|{invariantDate}|{invariantTime}";
    }
}
