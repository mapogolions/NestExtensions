using NestExtensions.Test.Fixtures;

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
    public async Task ShouldCheckClusterHealth()
    {
        var res = await _fixture.Client.Cluster.HealthAsync();
        Assert.True(res.IsValid);
    }
}
