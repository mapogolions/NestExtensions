using NestExtensions.Tests.Fixtures;
using Nest;

namespace NestExtensions.Tests;

[Collection(nameof(ElasticsearchCollection))]
public class PointInTimeReaderTests
{
    private readonly ElasticsearchFixture _fixture;

    public PointInTimeReaderTests(ElasticsearchFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldReadUsingMultipleSlices()
    {
        // Arrange
        string indexName = GenerateIndexName();
        const int expected = 10_000;
        var documents = Enumerable.Range(1, expected).Select(x => new Item { Id = x, Name = $"Item{x}" }).ToList();
        await _fixture.Client.IndexManyAsync(documents, indexName);
        await _fixture.Client.Indices.RefreshAsync(indexName);

        // Act
        await using var reader = await _fixture.Client.PointInTimeReader<Item>(indexName, size: 1000, slices: 4);
        var result = await Task.WhenAll(reader.Slices.Select(x => x.Documents()).ToArray());
        var actual = result.Sum(x => x.Count);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ShouldReadSpecifiedSizeAtOneTime()
    {
        // Arrange
        const int size = 200;
        string indexName = GenerateIndexName();
        var documents = Enumerable.Range(1, 1000).Select(x => new Item { Id = x, Name = $"Item{x}" }).ToList();
        await _fixture.Client.IndexManyAsync(documents, indexName);
        await _fixture.Client.Indices.RefreshAsync(indexName);

        // Act
        await using var reader = await _fixture.Client.PointInTimeReader<Item>(indexName, size: size, slices: 1);
        var slice = reader.Slices.Single();

        // Assert
        await foreach (var chunk in slice)
        {
            Assert.Equal(size, chunk.Count);
        }
    }

    [Fact]
    public async Task ShouldReadAllDocumentsUsingSingleSlice()
    {
        // Arrange
        string indexName = GenerateIndexName();
        const int expected = 10_000;
        var documents = Enumerable.Range(1, expected).Select(x => new Item { Id = x, Name = $"Item{x}" }).ToList();
        await _fixture.Client.IndexManyAsync(documents, indexName);
        await _fixture.Client.Indices.RefreshAsync(indexName);

        // Act
        await using var reader = await _fixture.Client.PointInTimeReader<Item>(indexName, size: 1000, slices: 1);
        var slice = reader.Slices.Single();
        var actual = await slice.Documents();

        // Assert
        Assert.Equal(expected, actual.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task ShouldCreateSpecifiedNumberOfSlices(int slices)
    {
        string indexName = GenerateIndexName();
        await _fixture.Client.Indices.CreateAsync(indexName);
        await using var reader = await _fixture.Client.PointInTimeReader<Item>(indexName, slices: slices);
        Assert.Equal(slices, reader.Slices.Count);
    }

    [Fact]
    public async Task ShouldCheckClusterHealth()
    {
        var res = await _fixture.Client.Cluster.HealthAsync();
        Assert.True(res.IsValid);
    }

    private string GenerateIndexName(string prefix = "index") => $"prefix-{Guid.NewGuid()}";
}
