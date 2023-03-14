namespace NestExtensions;

public interface IPointInTimeReader<TDocument> : IAsyncDisposable
{
    Task<IReadOnlyCollection<IElasticIndexSlice<TDocument>>> Slices(CancellationToken cancellation = default);
}
