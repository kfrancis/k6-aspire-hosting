using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.Aspire;

/// <summary>
///     Extension methods for adding Aspire metrics output to k6 resources.
/// </summary>
public static class AspireK6Extensions
{
    /// <summary>
    ///     Configures k6 to output metrics to the Aspire dashboard via OpenTelemetry.
    /// </summary>
    /// <param name="builder">The k6 resource builder.</param>
    /// <param name="configure">Optional configuration for the Aspire output.</param>
    /// <returns>The k6 resource builder for chaining.</returns>
    public static IResourceBuilder<K6Resource> WithAspireMetrics(
        this IResourceBuilder<K6Resource> builder,
        Action<AspireOutputOptions>? configure = null)
    {
        // Create and configure options
        var options = new AspireOutputOptions();
        configure?.Invoke(options);

        // Try to detect the Aspire OTLP endpoint from environment variables
        if (string.IsNullOrEmpty(options.OtlpEndpoint))
        {
            var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
            if (!string.IsNullOrEmpty(otlpEndpoint))
            {
                options.OtlpEndpoint = otlpEndpoint;
            }
            // If still not found, use the default host.docker.internal address
            else
            {
                options.OtlpEndpoint = "http://host.docker.internal:4317";
            }
        }

        // Create the provider and add it to the resource
        var provider = new AspireOutputProvider(options);
        builder.Resource.OutputProviders.Add(provider);

        // Configure k6 with the provider
        provider.Configure(builder.Resource, builder);

        return builder;
    }
}
