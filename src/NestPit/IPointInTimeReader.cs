namespace NestPit;

public interface IPointInTimeReader<TDocument> : IAsyncDisposable
{
    IReadOnlyCollection<IElasticIndexSlice<TDocument>> Slices { get; }
}
