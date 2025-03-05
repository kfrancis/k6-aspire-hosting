using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

/// <summary>
///     Represents a k6 resource.
/// </summary>
public class K6Resource : ContainerResource
{
    /// <summary>
    ///     Initializes a new instance of <see cref="K6Resource" />.
    /// </summary>
    /// <param name="name">
    ///     The name of the k6 resource.
    /// </param>
    /// <param name="options">
    ///     The options for the k6 resource.
    /// </param>
    public K6Resource(string name, K6Options options) : base(name)
    {
        Options = options;
        OutputProviders = new List<IK6OutputProvider>();
    }

    public K6Options Options { get; }

    /// <summary>
    ///     Gets the output providers for the k6 resource.
    /// </summary>
    public IList<IK6OutputProvider> OutputProviders { get; }

    /// <summary>
    ///     Gets or sets the endpoint reference for the k6 API.
    /// </summary>
    public EndpointReference? ApiEndpointReference { get; set; }
}
