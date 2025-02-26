using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.InfluxDB;

public static class InfluxDbK6Extensions
{
    private const int InfluxDbPort = 8086;

    /// <summary>
    ///     Adds an InfluxDB container resource to the <see cref="IDistributedApplicationBuilder" />.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the InfluxDB container resource will be added.
    /// </param>
    /// <param name="name">
    ///     The name of the InfluxDB container resource.
    /// </param>
    /// <param name="configure">
    ///     The options to configure the InfluxDB container resource.
    /// </param>
    /// <returns>
    ///     A reference to the <see cref="IResourceBuilder{InfluxDbResource}" /> for further resource configuration.
    /// </returns>
    public static IResourceBuilder<InfluxDbResource> AddInfluxDb(this IDistributedApplicationBuilder builder,
        string name, Action<InfluxDbOptions>? configure = null)
    {
        var options = new InfluxDbOptions();

        // Allow caller to configure
        configure?.Invoke(options);

        var resource = new InfluxDbResource(name, options);

        return builder.AddResource(resource)
            .WithImage(options.ImageConfig.Image)
            .WithImageRegistry(options.ImageConfig.Registry)
            .WithImageTag(options.ImageConfig.Tag)
            .WithEnvironment("INFLUXDB_DB", "k6")
            .WithHttpEndpoint(0, InfluxDbPort, InfluxDbResource.PrimaryEndpointName);
    }

    /// <summary>
    ///     Adds InfluxDB output to k6 load tests.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the InfluxDB output will be added.
    /// </param>
    /// <param name="options">
    ///     The options to configure the InfluxDB output.
    /// </param>
    /// <returns>
    ///     The <see cref="IResourceBuilder{K6Resource}" /> with the InfluxDB output added.
    /// </returns>
    public static IResourceBuilder<K6Resource> WithInfluxDbOutput(
        this IResourceBuilder<K6Resource> builder,
        InfluxDbOptions? options = null)
    {
        options ??= new InfluxDbOptions();

        var provider = new InfluxDbOutputProvider(options);
        builder.Resource.OutputProviders.Add(provider);

        return builder;
    }
}
