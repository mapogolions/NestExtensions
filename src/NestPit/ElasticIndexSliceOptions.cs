namespace NestPit;

public class ElasticIndexSliceOptions
{
    public ElasticIndexSliceOptions(int sliceId, int maxSlices, string indexName, TimeSpan keepAlive, int size, string pit)
    {
        if (sliceId < 0) throw new ArgumentOutOfRangeException(nameof(sliceId));
        if (maxSlices < 1) throw new ArgumentOutOfRangeException(nameof(maxSlices));
        if (sliceId >= maxSlices)
        {
            throw new ArgumentOutOfRangeException($"{nameof(sliceId)} must be less than {nameof(maxSlices)}");
        }
        SliceId = sliceId;
        MaxSlices = maxSlices;
        IndexName = indexName;
        KeepAlive = keepAlive;
        Size = size;
        SlicingEnabled = maxSlices > 1;
        Pit = pit;
    }

    public int SliceId { get; } = 0;
    public int MaxSlices { get; } = 1;
    public string IndexName { get; }
    public TimeSpan KeepAlive { get; } = TimeSpan.FromSeconds(30);
    public int Size { get; }
    public bool SlicingEnabled { get; }
    public string Pit { get; }
}
