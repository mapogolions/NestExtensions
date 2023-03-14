namespace NestExtensions;

public interface IPointInTimeReader<TDocument> : IAsyncDisposable
{
    Task<bool> OpenPit(CancellationToken cancellation = default);
    IReadOnlyCollection<IElasticIndexSlice<TDocument>> Slices { get; }
}
