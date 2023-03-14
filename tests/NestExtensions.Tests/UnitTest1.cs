using Nest;
using Testcontainers.Elasticsearch;

namespace NestExtensions.Tests;

public class UnitTest1
{
    private const string _elasticUserPassword = "s3cret";
    private const int _containerPort = 9200;

    [Fact]
    public async Task Test1()
    {
        await using var container = new ElasticsearchBuilder()
            .WithPortBinding(_containerPort, assignRandomHostPort: true)
            .WithPassword(_elasticUserPassword)
            .WithEnvironment("xpack.security.enabled", "false")
            .Build();
        await container.StartAsync();


        var settings = new ConnectionSettings(new Uri($"http://localhost:{container.GetMappedPublicPort(_containerPort)}"))
            .BasicAuthentication("elastic", _elasticUserPassword)
            .DefaultFieldNameInferrer(p => p);
        var client = new ElasticClient(settings);

        var res = await client.Cluster.HealthAsync();

        Assert.True(res.IsValid);
    }
}
