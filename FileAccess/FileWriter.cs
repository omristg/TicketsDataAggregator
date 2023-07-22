namespace TicketsDataAggregator.FileAccess;

public class FileWriter : IFileWriter
{
    public void Write(string text, string directory, string fileName)
    {
        var resultPath = Path.Combine(directory, fileName);
        File.WriteAllText(resultPath, text);
        Console.WriteLine("Result saved to " + resultPath);

    }
}
