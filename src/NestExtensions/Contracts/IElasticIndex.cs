using Nest;

namespace NestExtensions.Contracts;

public interface IElasticIndex<TDocument> where TDocument : class
{
    IElasticIndexSnapshot<TDocument> TakeSnapshot(int size, int slices, TimeSpan keepAlive);
}
