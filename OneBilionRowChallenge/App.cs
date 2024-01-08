using System.Globalization;
using System.Text;

namespace OneBilionRowChallenge;

public sealed class AppStream : IAsyncDisposable
{
    private readonly FileStream _fileStream;
    private readonly Dictionary<string, MeasurementTemperature> _measurements = new();
    

    public AppStream(string path)
    {
        _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
            FileOptions.SequentialScan);
    }
    
    public async Task Run()
    {
        await foreach (var measurement in ReadMeasurements())
        {
            Check(measurement);
        }
    }

    private void Check(Measurement measurement)
    {
        if (_measurements.TryGetValue(measurement.City, out var measurementTemperature))
        {
            measurementTemperature.Check(measurement);
        }
        else
        {
            _measurements.Add(measurement.City, new MeasurementTemperature(measurement.Temperature));
        }
    }
    

    public void Print()
    {
        foreach (var (city, measurementTemperature) in _measurements)
        {
            Console.WriteLine($"{city}={measurementTemperature.Min}/{measurementTemperature.Mean}/{measurementTemperature.Max}");
        }
    }
    
    public ValueTask DisposeAsync()
    {
        _measurements.Clear();
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