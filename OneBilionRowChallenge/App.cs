namespace OneBilionRowChallenge;

public sealed class App : IAsyncDisposable
{
    private readonly FileStream _fileStream;


    public App(string path)
    {
        _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
            FileOptions.SequentialScan);
    }
    public async Task Run()
    {
        
    }

    public ValueTask DisposeAsync()
    {
       return _fileStream.DisposeAsync();
    }
}