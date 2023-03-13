using System.Text;
using Nest;

namespace NestExtensions;

public class ElasticIndexSlice<TDocument> : ISlice<TDocument> where TDocument : class
{
    private readonly IElasticClient _client;
    private readonly ElasticIndexSliceOptions _options;

    public ElasticIndexSlice(IElasticClient client, ElasticIndexSliceOptions options)
    {
        _client = client;
        _options = options;
    }

    public int Id => _options.SliceId;

    public async IAsyncEnumerator<IReadOnlyCollection<TDocument>> GetAsyncEnumerator(CancellationToken cancellation = default)
    {
        ISearchResponse<TDocument>? searchResponse = null;
        while (true)
        {
            searchResponse = await _client.SearchAsync<TDocument>(s =>
            {
                string pit = searchResponse?.PointInTimeId ?? _options.Pit;
                if (_options.SlicingEnabled) s.Slice(x => x.Id(_options.SliceId).Max(_options.MaxSlices));
                s.PointInTime(pit, p => p.KeepAlive(_options.KeepAlive));
                s.Size(_options.Size);
                s.Sort(x => x.Ascending(SortSpecialField.ShardDocumentOrder));
                if (searchResponse != null) s.SearchAfter(searchResponse.Hits?.LastOrDefault()?.Sorts);
                return s;
            }, default);
            if (!searchResponse.IsValid) yield break;
            if (searchResponse.Documents.Count <= 0) yield break;
            var documents = searchResponse.Documents.ToList();
            yield return searchResponse.Documents.ToList();
        }
    }
}
