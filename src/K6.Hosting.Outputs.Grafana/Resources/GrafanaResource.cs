using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Outputs.Grafana.Resources;

/// <summary>
///     Represents a Grafana resource.
/// </summary>
public class GrafanaResource : ContainerResource, IResourceWithConnectionString
{
    internal const string PrimaryEndpointName = "http";

    private EndpointReference? _primaryEndpoint;

    /// <summary>
    ///     Initializes a new instance of <see cref="GrafanaResource" />.
    /// </summary>
    /// <param name="name">
    ///     The name of the Grafana resource.
    /// </param>
    public GrafanaResource(string name) : base(name)
    {
    }

    /// <summary>
    ///     Gets the primary endpoint for the Grafana resource.
    /// </summary>
    public EndpointReference PrimaryEndpoint
    {
        get => _primaryEndpoint ??= new EndpointReference(this, PrimaryEndpointName);
    }

    /// <summary>
    ///     Gets the connection string expression for the Grafana resource.
    /// </summary>
    public ReferenceExpression ConnectionStringExpression
    {
        get => ReferenceExpression.Create(
            $"http://{PrimaryEndpoint.Property(EndpointProperty.Host)}:{PrimaryEndpoint.Property(EndpointProperty.Port)}"
        );
    }
}
