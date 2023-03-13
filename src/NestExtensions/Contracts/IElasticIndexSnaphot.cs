using Nest;

namespace NestExtensions.Contracts;

public interface IElasticIndexSnapshot<TDocument> : IAsyncDisposable
{
    Task<IReadOnlyCollection<IElasticIndexSlice<TDocument>>> Slices(CancellationToken cancellation = default);
}
