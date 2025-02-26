using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Outputs.InfluxDB;

/// <summary>
///     The InfluxDb container resource
/// </summary>
public sealed class InfluxDbResource : ContainerResource, IResourceWithConnectionString
{
    internal const string PrimaryEndpointName = "http";

    private EndpointReference? _primaryEndpoint;

    /// <summary>
    ///     Initializes a new instance of <see cref="InfluxDbResource" />.
    /// </summary>
    /// <param name="name">
    ///     The name of the InfluxDb resource.
    /// </param>
    /// <param name="options">
    ///     The options for the InfluxDb resource.
    /// </param>
    public InfluxDbResource(string name, InfluxDbOptions options) : base(name)
    {
        Options = options;
    }

    /// <summary>
    ///     Gets the options for the InfluxDb resource.
    /// </summary>
    public InfluxDbOptions Options { get; }

    /// <summary>
    ///     Gets the primary endpoint for the InfluxDb resource.
    /// </summary>
    public EndpointReference PrimaryEndpoint
    {
        get => _primaryEndpoint ??= new EndpointReference(this, PrimaryEndpointName);
    }

    /// <summary>
    ///     Gets the connection string expression for the InfluxDb resource.
    /// </summary>
    public ReferenceExpression ConnectionStringExpression
    {
        get => ReferenceExpression.Create(
            $"http://{PrimaryEndpoint.Property(EndpointProperty.Host)}:{PrimaryEndpoint.Property(EndpointProperty.Port)}/k6"
        );
    }
}
