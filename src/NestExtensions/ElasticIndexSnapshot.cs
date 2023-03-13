using Nest;

namespace NestExtensions.Contracts;

public class ElasticIndexSnapshot<TDocument> : IElasticIndexSnapshot<TDocument> where TDocument : class
{
    private readonly IElasticClient _client;
    private readonly ElasticSnapshotOptions _options;
    private string? _pit;

    public ElasticIndexSnapshot(IElasticClient client, ElasticSnapshotOptions options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IReadOnlyCollection<IElasticIndexSlice<TDocument>>> Slices(CancellationToken cancellation = default)
    {
        Time keepAlive = _options.KeepAlive;
        var response = await _client.OpenPointInTimeAsync(_options.IndexName, o => o.KeepAlive(keepAlive.ToString()));
        if (!response.IsValid) return Array.Empty<IElasticIndexSlice<TDocument>>();
        _pit = response.Id;
        var slices = new List<ElasticIndexSlice<TDocument>>(_options.Slices);
        for (int i = 0; i < _options.Slices; i++)
        {
            slices.Add(new ElasticIndexSlice<TDocument>(
                client: _client,
                options: new ElasticIndexSliceOptions(
                    sliceId: i,
                    maxSlices: _options.Slices,
                    indexName: _options.IndexName!,
                    keepAlive: _options.KeepAlive,
                    size: _options.Size,
                    pit: _pit)));
        }
        return slices;
    }

    public async ValueTask DisposeAsync()
    {
        if (!string.IsNullOrEmpty(_pit))
        {
            await _client.ClosePointInTimeAsync(s => s.Id(_pit));
        }
    }
}
