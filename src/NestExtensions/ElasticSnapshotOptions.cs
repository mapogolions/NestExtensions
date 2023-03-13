namespace NestExtensions.Utop;

public class ElasticSnapshotOptions
{
    public string? IndexName { get; init; }
    public TimeSpan KeepAlive { get; init; } = TimeSpan.FromSeconds(30);
    public int Size { get; init; } = 10_000;
    public int Slices { get; init; } = 4;
}
