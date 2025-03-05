using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;
using K6.Hosting.Outputs.Grafana.Resources;

namespace K6.Hosting.Outputs.Grafana;

/// <summary>
///     Represents the options for the Grafana output provider.
/// </summary>
public class GrafanaOutputProvider : IK6OutputProvider
{
    private readonly GrafanaOptions _options;
    private IResourceBuilder<GrafanaResource>? _grafanaResource;

    /// <summary>
    ///     Initializes a new instance of <see cref="GrafanaOutputProvider" />.
    /// </summary>
    /// <param name="options">
    ///     The options for the Grafana output provider.
    /// </param>
    public GrafanaOutputProvider(GrafanaOptions options)
    {
        _options = options;
    }

    /// <summary>
    ///     Gets the name of the output provider.
    /// </summary>
    public string OutputName
    {
        get => "grafana";
    }

    /// <summary>
    ///     Gets the arguments for the output provider.
    /// </summary>
    /// <returns>
    ///     The arguments for the output provider.
    /// </returns>
    public string GetOutputArguments()
    {
        // Grafana isn't a direct k6 output but rather a visualization tool
        return string.Empty;
    }

    /// <summary>
    ///     Adds the required resources for the output provider.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the required resources will be added.
    /// </param>
    public void AddRequiredResources(IDistributedApplicationBuilder builder)
    {
        // Create the Grafana resource
        var resource = new GrafanaResource(_options.ResourceName);
        _grafanaResource = builder.AddResource(resource)
            .WithImage(_options.ImageConfig.Image)
            .WithImageRegistry(_options.ImageConfig.Registry)
            .WithImageTag(_options.ImageConfig.Tag)
            .WithEnvironment("GF_AUTH_ANONYMOUS_ORG_ROLE", "Admin")
            .WithEnvironment("GF_AUTH_ANONYMOUS_ENABLED", "true")
            .WithEnvironment("GF_AUTH_BASIC_ENABLED", "false")
            .WithEnvironment("GF_SERVER_SERVE_FROM_SUB_PATH", "true")
            .WithEndpoint(0, 3000, name: "http", scheme: "http")
            .WithContainerRuntimeArgs("--network-alias=grafana");

        // Add dashboard mounting if specified
        if (!string.IsNullOrEmpty(_options.DashboardsPath))
        {
            _grafanaResource.WithBindMount(
                Path.GetFullPath(_options.DashboardsPath),
                "/var/lib/grafana/dashboards"
            );
        }

        // Add dashboard config if specified
        if (!string.IsNullOrEmpty(_options.DashboardConfigPath))
        {
            _grafanaResource.WithBindMount(
                Path.GetFullPath(_options.DashboardConfigPath),
                "/etc/grafana/provisioning/dashboards/dashboard.yaml"
            );
        }
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
    ///     Thrown when the Grafana resource has not been created.
    /// </exception>
    public Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder)
    {
        if (_grafanaResource == null)
        {
            throw new InvalidOperationException("Grafana resource not created");
        }

        // Link k6 and Grafana
        builder.WithReference(_grafanaResource);

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Configure Grafana with a data source.
    /// </summary>
    /// <param name="dataSourceProvider">
    ///     The data source provider to configure Grafana with.
    /// </param>
    /// <param name="customConfigPath">
    ///     A custom path to a data source configuration file.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the Grafana resource has not been created.
    /// </exception>
    public void ConfigureDataSource(IK6DataSourceProvider dataSourceProvider, string? customConfigPath = null)
    {
        if (_grafanaResource == null)
        {
            throw new InvalidOperationException("Grafana resource not created");
        }

        // If a custom config path is provided, use that
        if (!string.IsNullOrEmpty(customConfigPath))
        {
            _grafanaResource.WithBindMount(
                Path.GetFullPath(customConfigPath),
                $"/etc/grafana/provisioning/datasources/{dataSourceProvider.DataSourceType}.yaml"
            );
            return;
        }

        // Otherwise, generate a config file dynamically
        var tempDir = Path.Combine(Path.GetTempPath(), "k6-grafana-configs");
        Directory.CreateDirectory(tempDir);

        var configFile = Path.Combine(tempDir, $"{dataSourceProvider.DataSourceType}-datasource.yaml");

        // Get the Grafana endpoint for configuration generation
        var grafanaEndpoint = _grafanaResource.GetEndpoint("http");

        // Generate the configuration with the connection string expression
        // rather than trying to resolve it immediately
        var config = dataSourceProvider.GenerateGrafanaDataSourceConfig(
            grafanaEndpoint.Property(EndpointProperty.Host).ToString() ?? "localhost",
            int.Parse(grafanaEndpoint.Property(EndpointProperty.Port)?.ToString() ?? "3000")
        );

        File.WriteAllText(configFile, config);

        // Mount the generated config
        _grafanaResource.WithBindMount(
            configFile,
            $"/etc/grafana/provisioning/datasources/{dataSourceProvider.DataSourceType}.yaml"
        );
    }
}
