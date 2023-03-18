namespace NestPit;

public class PointInTimeReaderOptions
{
    public string? IndexName { get; init; }
    public TimeSpan KeepAlive { get; init; } = TimeSpan.FromSeconds(30);
    public int Size { get; init; } = 10_000;
    public int Slices { get; init; } = 1;
}
