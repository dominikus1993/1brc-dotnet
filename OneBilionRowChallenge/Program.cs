// See https://aka.ms/new-console-template for more information

using OneBilionRowChallenge;

var path = args is { Length: > 0} ? args[0] : "./measurements.txt";

Console.WriteLine($"Reading file: {path}");

await using var app = new AppStream(path);

app.Run();

app.Print();
Environment.Exit(0);