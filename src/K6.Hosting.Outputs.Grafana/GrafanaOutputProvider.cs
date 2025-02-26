using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;
using K6.Hosting.Outputs.Grafana.Resources;

namespace K6.Hosting.Outputs.Grafana
{
    public class GrafanaOutputProvider : IK6OutputProvider
    {
        private readonly GrafanaOptions _options;
        private IResourceBuilder<GrafanaResource>? _grafanaResource;

        public GrafanaOutputProvider(GrafanaOptions options)
        {
            _options = options;
        }

        public string OutputName => "grafana";

        public string GetOutputArguments()
        {
            // Grafana isn't a direct k6 output but rather a visualization tool
            return string.Empty;
        }

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
                .WithEndpoint(0, 3000, name: "http", scheme: "http");

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

        public Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder)
        {
            if (_grafanaResource == null)
                throw new InvalidOperationException("Grafana resource not created");

            // Link k6 and Grafana
            builder.WithReference(_grafanaResource);

            return Task.CompletedTask;
        }

        public void ConfigureDataSource(IK6DataSourceProvider dataSourceProvider, string? customConfigPath = null)
        {
            if (_grafanaResource == null)
                throw new InvalidOperationException("Grafana resource not created");

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

    public static class GrafanaK6Extensions
    {
        public static IResourceBuilder<K6Resource> WithGrafanaDataSource(
            this IResourceBuilder<K6Resource> builder,
            string? datasourceConfigPath = null)
        {
            // Find any data source providers
            var dataSourceProviders = builder.Resource.OutputProviders
                .OfType<IK6DataSourceProvider>()
                .ToList();

            if (dataSourceProviders.Count == 0)
            {
                throw new InvalidOperationException(
                    "No data source providers found. Add an output provider first."
                );
            }

            // Configure Grafana with all available data sources
            foreach (var provider in dataSourceProviders)
            {
                // Use the provider interface to configure Grafana
            }

            return builder;
        }

        /// <summary>
        /// Add Grafana visualization to k6 load tests
        /// </summary>
        public static IResourceBuilder<K6Resource> WithGrafanaDashboard(
            this IResourceBuilder<K6Resource> builder,
            Action<GrafanaOptions>? configureOptions = null)
        {
            var options = new GrafanaOptions();
            configureOptions?.Invoke(options);

            var provider = new GrafanaOutputProvider(options);
            provider.AddRequiredResources(builder.ApplicationBuilder);

            // Store the provider in the k6 resource
            builder.Resource.OutputProviders.Add(provider);

            // Configure immediately
            provider.ConfigureAsync(builder.Resource, builder).GetAwaiter().GetResult();

            return builder;
        }

        /// <summary>
        /// Configure Grafana specifically with InfluxDB as a data source
        /// </summary>
        public static IResourceBuilder<K6Resource> WithGrafanaInfluxDbDatasource<TResource>(
            this IResourceBuilder<K6Resource> builder,
            IResourceBuilder<TResource> influxDbResource,
            string? customDataSourceConfigPath = null)
            where TResource : IResourceWithConnectionString
        {
            // Find the GrafanaOutputProvider in the k6 resource
            var grafanaProvider = builder.Resource.OutputProviders
                .OfType<GrafanaOutputProvider>()
                .FirstOrDefault();

            if (grafanaProvider == null)
            {
                throw new InvalidOperationException(
                    "Grafana output provider not found. Call WithGrafanaDashboard first."
                );
            }

            // Find any InfluxDB provider specifically
            var influxDbProvider = builder.Resource.OutputProviders
                .OfType<IK6DataSourceProvider>()
                .FirstOrDefault(p => p.DataSourceType == "influxdb");

            if (influxDbProvider == null)
            {
                throw new InvalidOperationException(
                    "InfluxDB output provider not found. Add an InfluxDB output provider first."
                );
            }

            // Configure Grafana with the InfluxDB data source
            grafanaProvider.ConfigureDataSource(influxDbProvider, customDataSourceConfigPath);

            return builder;
        }
    }
}
