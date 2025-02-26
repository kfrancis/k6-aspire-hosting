using System;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Core;

public interface IK6OutputProvider
{
    string OutputName { get; }
    string GetOutputArguments();
    void AddRequiredResources(IDistributedApplicationBuilder builder);
    Task ConfigureAsync(K6Resource k6Resource, IResourceBuilder<K6Resource> builder);
}