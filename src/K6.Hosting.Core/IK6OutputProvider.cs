using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

/// <summary>
///     Represents a k6 output provider.
/// </summary>
public interface IK6OutputProvider
{
    /// <summary>
    ///     Gets the name of the output provider.
    /// </summary>
    string OutputName { get; }

    /// <summary>
    ///     Gets the arguments for the output provider.
    /// </summary>
    /// <returns>
    ///     The arguments for the output provider.
    /// </returns>
    string GetOutputArguments();

    /// <summary>
    ///     Adds the required resources for the output provider.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the required resources will be added.
    /// </param>
    void AddRequiredResources(IDistributedApplicationBuilder builder);

    /// <summary>
    ///     Configures the output provider.
    /// </summary>
    /// <param name="k6Resource">
    ///     The <see cref="K6Resource" /> to which the output provider will be added.
    /// </param>
    /// <param name="builder">
    ///     The <see cref="IResourceBuilder{K6Resource}" /> to which the output provider will be added.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder);
}