namespace NestExtensions.Utop;

public static class SliceCollectionExtensions
{
    public static async Task<IReadOnlyCollection<T>> Documents<T>(this IElasticIndexSlice<T> slice, CancellationToken cancellation = default)
    {
        if (slice is null) throw new ArgumentNullException(nameof(slice));
        var documents = new List<T>();
        await foreach (var chunk in slice.WithCancellation(cancellation))
        {
            documents.AddRange(chunk);
        }
        return documents;
    }
}
