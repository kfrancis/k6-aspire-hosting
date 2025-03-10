using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.Aspire;

public class AspireOutputProvider : IK6OutputProvider
{
    private readonly AspireOutputOptions _options;

    /// <summary>
    ///     Initializes a new instance of <see cref="AspireOutputProvider" />.
    /// </summary>
    /// <param name="options">The options for the Aspire output provider.</param>
    public AspireOutputProvider(AspireOutputOptions options)
    {
        _options = options;
    }

    /// <summary>
    ///     Gets the name of the output provider.
    /// </summary>
    public string OutputName
    {
        get => "aspire";
    }

    /// <summary>
    ///     Gets the arguments for the output provider.
    /// </summary>
    /// <returns>The arguments for the output provider.</returns>
    public string GetOutputArguments()
    {
        // Configure the OpenTelemetry output format
        // The format is experimental-opentelemetry=<endpoint> for the output destination
        return $"experimental-opentelemetry={_options.OtlpEndpoint}";
    }

    /// <summary>
    ///     Adds the required resources for the output provider.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the required resources will be added.
    /// </param>
    public void AddRequiredResources(IDistributedApplicationBuilder builder)
    {
        // No additional resources needed - we'll use the built-in Aspire OTLP collector
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
    public void Configure(K6Resource k6Resource, IResourceBuilder<K6Resource> builder)
    {
        // Try multiple possible host addresses with explicit port resolution
        builder.WithEnvironment("K6_OUT", "experimental-opentelemetry");
        builder.WithEnvironment("K6_OTEL_EXPORTER_TYPE", "grpc");

        // Don't use host.docker.internal directly in the endpoint
        builder.WithEnvironment("K6_OTEL_GRPC_EXPORTER_ENDPOINT", "127.0.0.1:4317");

        // Essential for making OpenTelemetry work with unencrypted connections
        builder.WithEnvironment("K6_OTEL_GRPC_EXPORTER_INSECURE", "true");

        // Try multiple host resolution approaches
        builder.WithContainerRuntimeArgs("--add-host=host.docker.internal:host-gateway");

        // Increase timeouts to give more time for connection attempts
        builder.WithEnvironment("K6_OTLP_TIMEOUT", "30s");
        builder.WithEnvironment("K6_OTEL_FLUSH_INTERVAL", "5s");
        builder.WithEnvironment("K6_OTEL_EXPORT_INTERVAL", "5s");
    }
}
