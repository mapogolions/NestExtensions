using Nest;

namespace NestPit;

internal partial class PointInTimeReader<TDocument>
{
    private class ElasticIndexSlice : IElasticIndexSlice<TDocument>
    {
        private readonly PointInTimeReader<TDocument> _reader;

        public ElasticIndexSlice(int id, PointInTimeReader<TDocument> reader)
        {
            Id = id;
            _reader = reader;
        }

        public int Id { get; }

        public async IAsyncEnumerator<IReadOnlyCollection<TDocument>> GetAsyncEnumerator(CancellationToken cancellation = default)
        {
            ISearchResponse<TDocument>? searchResponse = null;
            while (true)
            {
                searchResponse = await _reader._client.SearchAsync<TDocument>(s =>
                {
                    string pit = searchResponse?.PointInTimeId ?? _reader._pit!;
                    if (_reader._options.Slices > 1) s.Slice(x => x.Id(Id).Max(_reader._options.Slices));
                    s.PointInTime(pit, p => p.KeepAlive(_reader._options.KeepAlive));
                    s.Size(_reader._options.Size);
                    if (_reader._builder is not null) s.Query(_reader._builder);
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
}
