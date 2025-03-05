namespace K6.Hosting.Outputs.Aspire;

/// <summary>
///     Options for the Aspire output provider.
/// </summary>
public class AspireOutputOptions
{
    /// <summary>
    ///     Gets or sets the OpenTelemetry Protocol (OTLP) endpoint URL to send metrics to.
    /// </summary>
    /// <remarks>
    ///     This is typically the Aspire OTLP collector endpoint.
    /// </remarks>
    public string OtlpEndpoint { get; set; } = "http://host.docker.internal:4317";

    /// <summary>
    ///     Gets or sets the headers to include in OTLP requests.
    /// </summary>
    public string? OtlpHeaders { get; set; }

    /// <summary>
    ///     Gets or sets the timeout for OTLP requests.
    /// </summary>
    public string? OtlpTimeout { get; set; }

    /// <summary>
    ///     Gets or sets the metrics schema to use.
    ///     Valid values: "prometheus" or "native"
    /// </summary>
    public string? MetricsSchema { get; set; }
}
