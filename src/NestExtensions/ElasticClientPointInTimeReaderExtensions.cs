using Nest;

namespace NestExtensions;

public static class ElasticClientPointInTimeReaderExtensions
{
    public static IPointInTimeReader<TDocument> PointInTimeReader<TDocument>(this IElasticClient client, PointInTimeReaderOptions options)
        where TDocument : class
    {
        ArgumentNullException.ThrowIfNull(client);
        return new PointInTimeReader<TDocument>(client, options);
    }

    public static IPointInTimeReader<TDocument> PointInTimeReader<TDocument>(this IElasticClient client, string indexName, int size = 10_000, int slices = 1)
        where TDocument : class
    {
        return PointInTimeReader<TDocument>(client, new() { IndexName = indexName, Size = size, Slices = slices });
    }
}
