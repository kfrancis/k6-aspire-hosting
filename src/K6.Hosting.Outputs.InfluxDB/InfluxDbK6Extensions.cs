using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.InfluxDB;

public static class InfluxDbK6Extensions
{
    private const int InfluxDbPort = 8086;

    /// <summary>
    /// Adds an InfluxDB resource to the application model
    /// </summary>
    /// <param name="builder">The distributed application builder</param>
    /// <param name="name">The name of the resource</param>
    /// <param name="configure">
    /// Configure options for the influxDb
    /// </param>
    /// <returns>A resource builder for further configuration</returns>
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
            .WithHttpEndpoint(0, InfluxDbPort, name: InfluxDbResource.PrimaryEndpointName);
    }

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