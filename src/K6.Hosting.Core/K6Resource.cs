using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

public class K6Resource : ContainerResource
{
    public K6Resource(string name, K6Options options) : base(name)
    {
        Options = options;
        OutputProviders = new List<IK6OutputProvider>();
    }

    public K6Options Options { get; }
    public IList<IK6OutputProvider> OutputProviders { get; }
    public EndpointReference? ApiEndpointReference { get; set; }
}