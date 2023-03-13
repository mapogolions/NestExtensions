using Nest;

namespace NestExtensions.Contracts;

public class ElasticIndex<TDocument> : IElasticIndex<TDocument> where TDocument : class
{
    private readonly IElasticClient _client;
    private readonly ElasticIndexOptions _options;

    public ElasticIndex(IElasticClient client, ElasticIndexOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        _client = client;
        _options = options;
    }

    public IElasticIndexSnapshot<TDocument> TakeSnapshot(int size, int slices, TimeSpan keepAlive)
    {
        var options = new ElasticSnapshotOptions
        {
            IndexName = _options.IndexName,
            Size = size,
            KeepAlive = keepAlive,
            Slices = slices
        };
        return new ElasticIndexSnapshot<TDocument>(_client, options);
    }
}
