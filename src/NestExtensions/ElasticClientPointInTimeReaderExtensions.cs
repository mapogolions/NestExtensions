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
}
