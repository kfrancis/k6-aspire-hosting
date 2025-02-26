namespace K6.Hosting.Core;

public class K6Options
{
    public string ScriptDirectory { get; set; } = string.Empty;
    public string ScriptFileName { get; set; } = string.Empty;
    public ContainerImageConfig ImageConfig => new()
    {
        Registry = "docker.io",
        Image = "grafana/k6",
        Tag = "latest"
    };
}