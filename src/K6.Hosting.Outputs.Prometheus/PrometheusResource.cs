using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

namespace K6.Hosting.Outputs.Prometheus;

public class PrometheusOptions
{
    /// <summary>
    ///     Container image config
    /// </summary>
    public ContainerImageConfig ImageConfig
    {
        get => new() { Registry = "docker.io", Image = "prom/prometheus", Tag = "latest" };
    }
}
public class PrometheusResource : ContainerResource
{
    public PrometheusResource(string name, string? entrypoint = null) : base(name, entrypoint)
    {
    }
}
