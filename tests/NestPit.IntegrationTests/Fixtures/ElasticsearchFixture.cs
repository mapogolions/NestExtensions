using Nest;
using Testcontainers.Elasticsearch;

namespace NestPit.IntegrationTests.Fixtures;

public class ElasticsearchFixture : IDisposable
{
    private const int _containerPort = 9200;
    private const string _password = "secret";

    private readonly Lazy<ElasticsearchContainer> _container;
    private readonly Lazy<IElasticClient> _client;

    public ElasticsearchFixture()
    {
        _container = new Lazy<ElasticsearchContainer>(ContainerFactory);
        _client = new Lazy<IElasticClient>(ClientFactory);
    }

    public ElasticsearchContainer Container => _container.Value;

    private ElasticsearchContainer ContainerFactory()
    {
        var container = new ElasticsearchBuilder()
            .WithPortBinding(_containerPort, true)
            .WithPassword(_password)
            .WithEnvironment("xpack.security.enabled", "false")
            .Build();
        container.StartAsync().Wait();
        return container;
    }

    public IElasticClient Client =>  _client.Value;

    private IElasticClient ClientFactory()
    {
        var settings = new ConnectionSettings(new Uri($"http://localhost:{Container.GetMappedPublicPort(9200)}"))
            .BasicAuthentication("elastic", _password)
            .DefaultFieldNameInferrer(p => p);
        return new ElasticClient(settings);
    }

    public void Dispose()
    {
        Container.StopAsync().Wait();
    }
}

[CollectionDefinition(nameof(ElasticsearchCollection))]
public class ElasticsearchCollection : ICollectionFixture<ElasticsearchFixture> {}
