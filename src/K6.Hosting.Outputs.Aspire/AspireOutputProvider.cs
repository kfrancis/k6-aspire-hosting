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
        // Get the actual OTLP endpoint from the environment if possible
        var otlpEndpointFull = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")
                               ?? _options.OtlpEndpoint;

        // Parse the endpoint to extract just host:port for gRPC
        var grpcEndpoint = Uri.TryCreate(otlpEndpointFull, UriKind.Absolute, out var uri)
            ? $"{uri.Host}:{uri.Port}"
            : "host.docker.internal:4317";

        builder.WithEnvironment("K6_OTEL_METRIC_PREFIX", "k6");

        // Configure k6 to use the OTLP endpoint
        builder.WithEnvironment("K6_OUT", "experimental-opentelemetry");

        // Add additional environment variables for more reliable connectivity
        builder.WithEnvironment("K6_OTLP_TIMEOUT", _options.OtlpTimeout ?? "20s");

        // Set the exporter type (gRPC is usually used for OTLP)
        builder.WithEnvironment("K6_OTEL_EXPORTER_TYPE", "grpc");

        // Configure the gRPC endpoint for OpenTelemetry
        builder.WithEnvironment("K6_OTEL_GRPC_EXPORTER_ENDPOINT", grpcEndpoint);

        // Since we're using HTTP and not HTTPS, disable security
        builder.WithEnvironment("K6_OTEL_GRPC_EXPORTER_INSECURE", "true");

        // Add reasonable timeout values
        builder.WithEnvironment("K6_OTEL_FLUSH_INTERVAL", "5s");
        builder.WithEnvironment("K6_OTEL_EXPORT_INTERVAL", "5s");

        // Ensure we can connect to the host
        builder.WithContainerRuntimeArgs("--add-host=host.docker.internal:host-gateway");
    }
}
