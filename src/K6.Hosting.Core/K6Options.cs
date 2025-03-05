namespace K6.Hosting.Core;

/// <summary>
///     Represents the options for a k6 resource.
/// </summary>
public class K6Options
{
    /// <summary>
    ///     Gets or sets the directory where the k6 script is located.
    /// </summary>
    public string ScriptDirectory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the k6 script file.
    /// </summary>
    public string ScriptFileName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the configuration for the container image.
    /// </summary>
    public ContainerImageConfig ImageConfig
    {
        get => new() { Registry = "docker.io", Image = "grafana/k6", Tag = "latest" };
    }
}
