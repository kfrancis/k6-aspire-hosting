using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.Grafana;

/// <summary>
///     Represents the options for the Grafana output provider.
/// </summary>
public static class GrafanaK6Extensions
{
    /// <summary>
    ///     Add Grafana visualization to k6 load tests
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the Grafana visualization will be added.
    /// </param>
    /// <param name="datasourceConfigPath">
    ///     The path to the data source configuration file.
    /// </param>
    /// <returns>
    ///     The <see cref="IResourceBuilder{K6Resource}" /> with the Grafana visualization added.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no data source providers are found. Add an output provider first.
    /// </exception>
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
    ///     Add Grafana visualization to k6 load tests
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the Grafana visualization will be added.
    /// </param>
    /// <param name="configureOptions">
    ///     The options to configure the Grafana visualization.
    /// </param>
    /// <returns>
    ///     The <see cref="IResourceBuilder{K6Resource}" /> with the Grafana visualization added.
    /// </returns>
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
        provider.Configure(builder.Resource, builder);

        return builder;
    }

    /// <summary>
    ///     Add Grafana visualization to k6 load tests
    /// </summary>
    /// <typeparam name="TResource">
    ///     The type of the InfluxDB resource.
    /// </typeparam>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the Grafana visualization will be added.
    /// </param>
    /// <param name="influxDbResource">
    ///     The <see cref="IResourceBuilder{TResource}" /> that represents the InfluxDB resource.
    /// </param>
    /// <param name="customDataSourceConfigPath">
    ///     A custom path to the data source configuration file.
    /// </param>
    /// <returns>
    ///     The <see cref="IResourceBuilder{K6Resource}" /> with the Grafana visualization added.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the Grafana output provider is not found. Call WithGrafanaDashboard first.
    /// </exception>
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
