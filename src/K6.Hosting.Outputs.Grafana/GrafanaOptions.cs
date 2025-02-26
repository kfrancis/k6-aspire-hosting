using K6.Hosting.Core;

namespace K6.Hosting.Outputs.Grafana;

public class GrafanaOptions
{
    /// <summary>
    /// Name for the Grafana resource
    /// </summary>
    public string ResourceName { get; set; } = "grafana";

    /// <summary>
    /// Custom path to Grafana dashboards
    /// </summary>
    public string? DashboardsPath { get; set; }

    /// <summary>
    /// Path to custom datasource config
    /// </summary>
    public string? DatasourceConfigPath { get; set; }

    /// <summary>
    /// Path to custom dashboard config
    /// </summary>
    public string? DashboardConfigPath { get; set; }

    /// <summary>
    /// Container image config
    /// </summary>
    public ContainerImageConfig ImageConfig { get; set; } = new()
    {
        Registry = "docker.io",
        Image = "grafana/grafana",
        Tag = "latest"
    };
}