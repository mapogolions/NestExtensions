namespace NestExtensions;

public static class ElasticIndexSliceExtensions
{
    public static async Task<IReadOnlyCollection<T>> Documents<T>(this IElasticIndexSlice<T> slice, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(slice);
        var documents = new List<T>();
        await foreach (var chunk in slice)
        {
            documents.AddRange(chunk);
        }
        return documents;
    }
}
