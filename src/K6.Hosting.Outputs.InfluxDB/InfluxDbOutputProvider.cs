using System;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;
using K6.Hosting.Outputs.Grafana;

namespace K6.Hosting.Outputs.InfluxDB;

public class InfluxDbOutputProvider : IK6OutputProvider, IK6DataSourceProvider
{
    private readonly InfluxDbOptions _options;
    private IResourceBuilder<InfluxDbResource>? _influxDbResource;

    public InfluxDbOutputProvider(InfluxDbOptions options)
    {
        _options = options;
    }

    public string OutputName => "influxdb";

    public string GetOutputArguments()
    {
        return $"influxdb={_options.ConnectionString}";
    }

    public void AddRequiredResources(IDistributedApplicationBuilder builder)
    {
        // Create InfluxDB resource and store the reference
        _influxDbResource = builder.AddInfluxDb(_options.ResourceName ?? "influxdb");

        // Configure as needed
        _influxDbResource.WithHttpEndpoint(port: 8086);
    }

    public Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder)
    {
        if (_influxDbResource == null)
            throw new InvalidOperationException("InfluxDB resource not created");

        // Add references and configure k6 for InfluxDB output
        builder.WithReference(_influxDbResource);

        // Set environment variables
        builder.WithEnvironment("K6_OUT", GetOutputArguments());

        return Task.CompletedTask;
    }

    public string DataSourceType => "influxdb";
    public ReferenceExpression ConnectionStringExpression =>
        _influxDbResource?.Resource is IResourceWithConnectionString resourceWithConn
            ? resourceWithConn.ConnectionStringExpression
            : throw new InvalidOperationException("InfluxDB resource not created or doesn't support connection strings");

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
}

/// <summary>
/// The InfluxDb container resource 
/// </summary>
public sealed class InfluxDbResource : ContainerResource, IResourceWithConnectionString
{
    internal const string PrimaryEndpointName = "http";

    public InfluxDbResource(string name, InfluxDbOptions options) : base(name)
    {
        Options = options;
    }

    public InfluxDbOptions Options { get; }

    private EndpointReference? _primaryEndpoint;
    public EndpointReference PrimaryEndpoint => _primaryEndpoint ??= new EndpointReference(this, PrimaryEndpointName);

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"http://{PrimaryEndpoint.Property(EndpointProperty.Host)}:{PrimaryEndpoint.Property(EndpointProperty.Port)}/k6"
        );
}