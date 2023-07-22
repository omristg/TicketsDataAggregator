namespace TicketsDataAggregator.FileAccess;

public interface IFileWriter
{
    void Write(string text, string directory, string fileName);
}
