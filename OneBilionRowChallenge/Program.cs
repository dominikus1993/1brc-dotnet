// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using OneBilionRowChallenge;

var sw = Stopwatch.StartNew();
var path = args is { Length: > 1} ? args[0] : "./measurements.txt";
await using var app = new AppStream(path);

await app.Run();

app.Print();

sw.Stop();

Console.WriteLine($"Elapsed time: {sw.Elapsed}");
Environment.Exit(0);