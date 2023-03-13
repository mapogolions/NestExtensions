namespace NestExtensions.Utop;

public interface IElasticIndexSlice<T> : IAsyncEnumerable<IReadOnlyCollection<T>>
{
    int Id { get; }
}
