using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Outputs.Grafana.Resources
{
    public class GrafanaResource : ContainerResource, IResourceWithConnectionString
    {
        internal const string PrimaryEndpointName = "http";

        public GrafanaResource(string name) : base(name)
        {
        }

        private EndpointReference? _primaryEndpoint;
        public EndpointReference PrimaryEndpoint => _primaryEndpoint ??= new EndpointReference(this, PrimaryEndpointName);

        public ReferenceExpression ConnectionStringExpression =>
            ReferenceExpression.Create(
                $"http://{PrimaryEndpoint.Property(EndpointProperty.Host)}:{PrimaryEndpoint.Property(EndpointProperty.Port)}"
            );
    }
}
