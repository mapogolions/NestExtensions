using Nest;
using NestExtensions;

namespace NestExtensions;

public static class ElasticClientTakeSnapshotExtensions
{
    public static IPointInTimeReader<TDocument> PointInTimeReader<TDocument>(this IElasticClient client, PointInTimeReaderOptions options)
        where TDocument : class
    {
        ArgumentNullException.ThrowIfNull(client);
        return new PointInTimeReader<TDocument>(client, options);
    }
}
