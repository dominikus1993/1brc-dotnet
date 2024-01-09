using System.Diagnostics;
using System.Globalization;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Text;
using csFastFloat;

namespace OneBilionRowChallenge;

public sealed class AppStream : IDisposable
{
    private readonly FileStream _fileStream;
    private readonly Dictionary<string, MeasurementTemperature> _measurements = new();
    private const int BufferSize = 512 * 1024;
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
        var line = reader.ReadLine();
        while (line is not null)
        {
            _count++;
            var parts = FastSplit(line, ';');
            Check(parts.city, parts.temp);
            line = reader.ReadLine();
        }
        return;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (string city, double temp) FastSplit(string txt, char separator)
    {
        var span = txt.AsSpan();
        for (int i = 0; i < txt.Length; i++)
        {
            if (span[i] == separator)
            {
                return (new string(span[..i]), FastDoubleParser.ParseDouble(span[(i + 1)..]));
            }
        }

        throw new InvalidOperationException("cannot split");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Check(string city, double temperature)
    {
        if (_measurements.TryGetValue(city, out var measurementTemperature))
        {
            measurementTemperature.Check(temperature);
        }
        else
        {
            _measurements[city] = new MeasurementTemperature(temperature);
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
    
    public void Dispose()
    {
        _measurements.Clear();
        _fileStream.Flush();
       _fileStream.Dispose();
    }
}