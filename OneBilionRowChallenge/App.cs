using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace OneBilionRowChallenge;

public sealed class AppStream : IAsyncDisposable
{
    private readonly FileStream _fileStream;
    private readonly Dictionary<string, MeasurementTemperature> _measurements = new();
    private const int BufferSize = 256 * 1024;
    
    private int _count;
    private readonly Stopwatch _sw;

    public AppStream(string path)
    {
        _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize,
            FileOptions.SequentialScan);
        _count = 0;
        _sw = Stopwatch.StartNew();
    }
    
    public void Run()
    {
        using var reader = new StreamReader(_fileStream, Encoding.UTF8, true, BufferSize, true);
        while (reader.ReadLine() is {} line)
        {
            var parts = line.Split(';');
            var measurement = new Measurement
            {
                City = parts[0],
                Temperature = double.Parse(parts[1], CultureInfo.InvariantCulture)
            };
            _count++;
            Check(measurement);
        }

        return;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Check(Measurement measurement)
    {
        if (_measurements.TryGetValue(measurement.City, out var measurementTemperature))
        {
            measurementTemperature.Check(measurement);
        }
        else
        {
            _measurements[measurement.City] = new MeasurementTemperature(measurement.Temperature);
        }
    }
    

    public void Print()
    {
        _sw.Stop();
        foreach (var (city, measurementTemperature) in _measurements)
        {
            Console.WriteLine($"{city}={measurementTemperature.Min}/{measurementTemperature.Mean}/{measurementTemperature.Max}");
        }
        
        Console.WriteLine($"Elapsed time: {_sw.Elapsed}");
        var entriesPerSecond = _count / _sw.Elapsed.TotalSeconds;
        Console.WriteLine($"Entries per second: {entriesPerSecond}");
        var estimatedTime = TimeSpan.FromSeconds(1_000_000_000 / entriesPerSecond);
        Console.WriteLine($"Estimated time for 1 billion entries: {estimatedTime}");
        
    }
    
    public ValueTask DisposeAsync()
    {
        _measurements.Clear();
       return _fileStream.DisposeAsync();
    }
}