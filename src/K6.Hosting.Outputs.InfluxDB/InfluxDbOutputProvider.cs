using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.InfluxDB;

/// <summary>
///     The InfluxDB output provider
/// </summary>
public class InfluxDbOutputProvider : IK6OutputProvider, IK6DataSourceProvider
{
    private readonly InfluxDbOptions _options;
    private IResourceBuilder<InfluxDbResource>? _influxDbResource;

    /// <summary>
    ///     Initializes a new instance of <see cref="InfluxDbOutputProvider" />.
    /// </summary>
    /// <param name="options">
    ///     The options for the InfluxDB output provider.
    /// </param>
    public InfluxDbOutputProvider(InfluxDbOptions options)
    {
        _options = options;
    }

    /// <summary>
    ///     The type of data source provided by this output provider.
    /// </summary>
    public string DataSourceType
    {
        get => "influxdb";
    }

    /// <summary>
    ///     The InfluxDB resource created by this output provider.
    /// </summary>
    public ReferenceExpression ConnectionStringExpression
    {
        get => _influxDbResource?.Resource is IResourceWithConnectionString resourceWithConn
            ? resourceWithConn.ConnectionStringExpression
            : throw new InvalidOperationException(
                "InfluxDB resource not created or doesn't support connection strings");
    }

    /// <summary>
    ///     Generates a Grafana data source configuration for this InfluxDB.
    /// </summary>
    /// <param name="grafanaHost">
    ///     The host where Grafana is running.
    /// </param>
    /// <param name="grafanaPort">
    ///     The port where Grafana is running.
    /// </param>
    /// <returns>
    ///     The Grafana data source configuration.
    /// </returns>
    public string GenerateGrafanaDataSourceConfig(string grafanaHost, int grafanaPort)
    {
        // Generate YAML or JSON config for Grafana to connect to this InfluxDB
        // This can be written to a file or mounted directly
        return $"""
                apiVersion: 1
                datasources:
                  - name: InfluxDB
                    type: influxdb
                    access: proxy
                    url: {ConnectionStringExpression}
                    database: k6
                    isDefault: true
                """;
    }

    /// <summary>
    ///     Gets the name of the output provider.
    /// </summary>
    public string OutputName
    {
        get => "influxdb";
    }

    /// <summary>
    ///     Gets the arguments for the output provider.
    /// </summary>
    /// <returns></returns>
    public string GetOutputArguments()
    {
        return $"influxdb={_options.ConnectionString}";
    }

    /// <summary>
    ///     Adds the required resources for the output provider.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the required resources will be added.
    /// </param>
    public void AddRequiredResources(IDistributedApplicationBuilder builder)
    {
        // Create InfluxDB resource and store the reference
        _influxDbResource = builder.AddInfluxDb(_options.ResourceName ?? "influxdb");

        // Configure as needed
        _influxDbResource.WithHttpEndpoint(8086);
    }

    /// <summary>
    ///     Configures the output provider.
    /// </summary>
    /// <param name="k6Resource">
    ///     The <see cref="K6Resource" /> to which the output provider will be added.
    /// </param>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the output provider will be added.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the InfluxDB resource is not created.
    /// </exception>
    public Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder)
    {
        if (_influxDbResource == null)
        {
            throw new InvalidOperationException("InfluxDB resource not created");
        }

        // Add references and configure k6 for InfluxDB output
        builder.WithReference(_influxDbResource);

        // Set environment variables
        builder.WithEnvironment("K6_OUT", GetOutputArguments());

        return Task.CompletedTask;
    }
}
