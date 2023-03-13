namespace NestExtensions;

public interface IElasticIndexSlice<T> : IAsyncEnumerable<IReadOnlyCollection<T>>
{
    int Id { get; }
}
