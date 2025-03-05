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
            .WithHttpEndpoint(0, InfluxDbPort, InfluxDbResource.PrimaryEndpointName)
            .WithContainerRuntimeArgs($"--network-alias={name}");
    }

    /// <summary>
    ///     Adds InfluxDB output to k6 load tests.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the InfluxDB output will be added.
    /// </param>
    /// <param name="influxDbResource">
    ///    The InfluxDB container resource to use for output.
    /// </param>
    /// <returns>
    ///     The <see cref="IResourceBuilder{K6Resource}" /> with the InfluxDB output added.
    /// </returns>
    public static IResourceBuilder<K6Resource> WithInfluxDbOutput(
        this IResourceBuilder<K6Resource> builder,
        IResourceBuilder<InfluxDbResource> influxDbResource)
    {
        var provider = new InfluxDbOutputProvider(new InfluxDbOptions());
        builder.Resource.OutputProviders.Add(provider);

        // Need to set the out influxdb output
        var endpointReference = influxDbResource.GetEndpoint(InfluxDbResource.PrimaryEndpointName);
        var influxDbUrl = $"influxdb=http://influxdb:{endpointReference.TargetPort}/k6";
        builder.WithReference(influxDbResource);
        builder.WithEnvironment("K6_OUT", influxDbUrl);

        return builder;
    }
}
