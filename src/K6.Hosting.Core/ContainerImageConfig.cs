namespace K6.Hosting.Core;

/// <summary>
///    Represents the configuration for a container image.
/// </summary>
public class ContainerImageConfig
{
    /// <summary>
    ///    Gets or sets the registry for the container image.
    /// </summary>
    public string Registry { get; set; } = string.Empty;

    /// <summary>
    ///   Gets or sets the image for the container.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    ///   Gets or sets the tag for the container image.
    /// </summary>
    public string Tag { get; set; } = string.Empty;
}
