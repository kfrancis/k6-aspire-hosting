using K6.Hosting.Core;

namespace K6.Hosting.Outputs.InfluxDB;

public class InfluxDbOptions
{
    public string? ConnectionString { get; set; }
    public string? ResourceName { get; internal set; }

    public ContainerImageConfig ImageConfig => new()
    {
        Registry = "docker.io",
        Image = "influxdb",
        Tag = "1.8"
    };
}