using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

/// <summary>
///     Represents a k6 data source provider.
/// </summary>
public interface IK6DataSourceProvider
{
    /// <summary>
    ///     Gets the name of the data source provider.
    /// </summary>
    string DataSourceType { get; }

    /// <summary>
    ///     Gets the connection string expression for the data source provider.
    /// </summary>
    ReferenceExpression ConnectionStringExpression { get; }

    /// <summary>
    ///     Generates the Grafana data source configuration.
    /// </summary>
    /// <param name="grafanaHost">
    ///     The host of the Grafana instance.
    /// </param>
    /// <param name="grafanaPort">
    ///     The port of the Grafana instance.
    /// </param>
    /// <returns>
    ///     The Grafana data source configuration.
    /// </returns>
    string GenerateGrafanaDataSourceConfig(string grafanaHost, int grafanaPort);
}