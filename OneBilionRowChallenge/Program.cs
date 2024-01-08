// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using OneBilionRowChallenge;

var sw = Stopwatch.StartNew();
var path = args is { Length: > 1} ? args[0] : "./measurements.txt";
using var app = new App();

await app.Run(path);

sw.Stop();

Console.WriteLine($"Elapsed time: {sw.Elapsed}");
Environment.Exit(0);