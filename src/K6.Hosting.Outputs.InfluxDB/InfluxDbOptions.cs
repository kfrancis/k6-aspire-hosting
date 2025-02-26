using K6.Hosting.Core;

namespace K6.Hosting.Outputs.InfluxDB;

/// <summary>
///     Represents the options for the InfluxDB output provider.
/// </summary>
public class InfluxDbOptions
{
    /// <summary>
    ///     Gets or sets the connection string for the InfluxDB resource.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    ///     Name for the InfluxDB resource
    /// </summary>
    public string? ResourceName { get; internal set; }

    /// <summary>
    ///     Container image config
    /// </summary>
    public ContainerImageConfig ImageConfig
    {
        get => new() { Registry = "docker.io", Image = "influxdb", Tag = "1.8" };
    }
}
