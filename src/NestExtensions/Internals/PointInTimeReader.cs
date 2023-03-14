using Nest;

namespace NestExtensions;

internal partial class PointInTimeReader<TDocument> : IPointInTimeReader<TDocument> where TDocument : class
{
    private readonly Lazy<IReadOnlyCollection<IElasticIndexSlice<TDocument>>> _slices;
    private readonly IElasticClient _client;
    private readonly PointInTimeReaderOptions _options;
    private readonly string _pit;
    private bool _disposed;

    internal PointInTimeReader(IElasticClient client, PointInTimeReaderOptions options, string pit)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _pit = pit ?? throw new ArgumentNullException(nameof(_pit));
        _slices = new(Factory, isThreadSafe: false);
    }

    public IReadOnlyCollection<IElasticIndexSlice<TDocument>> Slices
    {
        get
        {
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name);
            return _slices.Value;
        }
    }

    private IReadOnlyCollection<IElasticIndexSlice<TDocument>> Factory()
    {
        var slices = new List<ElasticIndexSlice>(_options.Slices);
        for (int i = 0; i < _options.Slices; i++)
        {
            slices.Add(new ElasticIndexSlice(i, this));
        }
        return slices;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        if (!string.IsNullOrEmpty(_pit))
        {
            await _client.ClosePointInTimeAsync(s => s.Id(_pit));
            _disposed = true;
        }
    }
}
