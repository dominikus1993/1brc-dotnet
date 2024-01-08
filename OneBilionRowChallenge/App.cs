using System.Globalization;
using System.Text;

namespace OneBilionRowChallenge;

public sealed class AppStream : IAsyncDisposable
{
    private readonly FileStream _fileStream;


    public AppStream(string path)
    {
        _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
            FileOptions.SequentialScan);
    }
    
    public async Task Run()
    {
        await foreach (var measurement in ReadMeasurements().Take(1))
        {
            Console.WriteLine($"{measurement.City} {measurement.Temperature}");
        }
    }

    public ValueTask DisposeAsync()
    {
       return _fileStream.DisposeAsync();
    }
    
    private async IAsyncEnumerable<Measurement> ReadMeasurements()
    {
        using var reader = new StreamReader(_fileStream, Encoding.UTF8, true, 4096, true);
        while (await reader.ReadLineAsync() is {} line)
        {
            var parts = line.Split(';');
            yield return new Measurement
            {
                City = parts[0],
                Temperature = double.Parse(parts[1], CultureInfo.InvariantCulture)
            };
        }
    }
}