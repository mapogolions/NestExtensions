namespace NestExtensions.Contracts;

public interface IPointInTimeReader<TDocument> : IAsyncDisposable
{
    Task<IReadOnlyCollection<IElasticIndexSlice<TDocument>>> Slices(CancellationToken cancellation = default);
}
