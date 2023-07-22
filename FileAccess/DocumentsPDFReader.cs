

using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;

namespace TicketsDataAggregator.FileAccess;

public class DocumentsPDFReader : IDocumentsReader
{
    public IEnumerable<string> Read(string directory)
    {
        var pdfFiles = Directory.GetFiles(directory, "*.pdf");
        foreach (var filePath in pdfFiles)
        {
            using PdfDocument document = PdfDocument.Open(filePath);
            Page page = document.GetPage(1);
            yield return page.Text;
        }
    }

}