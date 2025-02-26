using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

public interface IK6DataSourceProvider
{
    string DataSourceType { get; }
    ReferenceExpression ConnectionStringExpression { get; }
    string GenerateGrafanaDataSourceConfig(string grafanaHost, int grafanaPort);
}