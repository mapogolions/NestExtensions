using Nest;

namespace NestExtensions;

public static class ElasticClientPointInTimeReaderExtensions
{
    public static async Task<IPointInTimeReader<TDocument>> PointInTimeReader<TDocument>(
        this IElasticClient client,
        PointInTimeReaderOptions options,
        Func<SearchDescriptor<TDocument>, ISearchRequest>? configure = null,
        CancellationToken cancellation = default) where TDocument : class
    {
        ArgumentNullException.ThrowIfNull(client);
        Time keepAlive = options.KeepAlive;
        var response = await client.OpenPointInTimeAsync(options.IndexName, o => o.KeepAlive(keepAlive.ToString()));
        return new PointInTimeReader<TDocument>(client, options, response.Id, configure);
    }

    public static Task<IPointInTimeReader<TDocument>> PointInTimeReader<TDocument>(
        this IElasticClient client,
        string indexName,
        int size = 10_000,
        int slices = 1,
        Func<SearchDescriptor<TDocument>, ISearchRequest>? configure = null,
        CancellationToken cancellation = default)
        where TDocument : class
    {
        return PointInTimeReader<TDocument>(client, new() { IndexName = indexName, Size = size, Slices = slices }, configure, cancellation);
    }

    public static Task<IPointInTimeReader<TDocument>> PointInTimeReader<TDocument>(
        this IElasticClient client,
        string indexName,
        TimeSpan keepAlive,
        int size = 10_000,
        int slices = 1,
        Func<SearchDescriptor<TDocument>, ISearchRequest>? configure = null,
        CancellationToken cancellation = default)
        where TDocument : class
    {
        return PointInTimeReader<TDocument>(client, new() { IndexName = indexName, Size = size, Slices = slices, KeepAlive = keepAlive }, configure, cancellation);
    }
}
