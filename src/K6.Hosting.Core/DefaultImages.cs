namespace K6.Hosting.Core;

public class ContainerImageConfig
{
    public string Registry { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
}

public static class DefaultImages
{
    public static ContainerImageConfig K6 => new()
    {
        Registry = "docker.io",
        Image = "grafana/k6",
        Tag = "latest"
    };

    
}