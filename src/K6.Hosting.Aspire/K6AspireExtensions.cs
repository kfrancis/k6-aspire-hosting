using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace K6.Hosting.Aspire;

public static class K6AspireExtensions
{
    private const int K6Port = 6565;

    /// <summary>
    ///     Adds a k6 container resource to the <see cref="IDistributedApplicationBuilder" />.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the k6 container resource will be
    ///     added.
    /// </param>
    /// <param name="name">The name of the k6 container resource.</param>
    /// <param name="configure"></param>
    /// <returns>A reference to the <see cref="IResourceBuilder{K6Resource}" /> for further resource configuration.</returns>
    public static IResourceBuilder<K6Resource> AddK6(this IDistributedApplicationBuilder builder, string name,
        Action<K6Options>? configure = null)
    {
        var options = new K6Options();

        // Allow caller to configure
        configure?.Invoke(options);

        // Make sure we have script info
        if (string.IsNullOrWhiteSpace(options.ScriptDirectory))
        {
            throw new ArgumentException("ScriptDirectory must be provided.", nameof(options.ScriptDirectory));
        }
        if (string.IsNullOrWhiteSpace(options.ScriptFileName))
        {
            throw new ArgumentException("ScriptFileName must be provided.", nameof(options.ScriptFileName));
        }

        // Convert to absolute paths
        var scriptDir = Path.GetDirectoryName(Path.GetFullPath(options.ScriptDirectory));
        var scriptFileName = Path.GetFileName(options.ScriptFileName);

        var resource = new K6Resource(name, options);

        // let's use the k6 docker image here
        var resourceBuilder = builder.AddResource(resource)
            .WithImage(options.ImageConfig.Image)
            .WithImageRegistry(options.ImageConfig.Registry)
            .WithImageTag(options.ImageConfig.Tag)
            .WithEnvironment("K6_INSECURE_SKIP_TLS_VERIFY", "true")
            .WithEndpoint(0, K6Port, name: "k6-api")
            .WithArgs("run", $"/scripts/{scriptFileName}")
            .WithExplicitStart();

        if (scriptDir != null)
            resourceBuilder.WithBindMount(scriptDir, "/scripts");

        return resourceBuilder;
    }

    public static IResourceBuilder<K6Resource> WithApiEndpoint(this IResourceBuilder<K6Resource> builder,
        IResourceBuilder<ProjectResource> apiProject)
    {
        // Get the endpoint reference from the API project
        var endpointReference = apiProject.GetEndpoint("https");

        // Store the endpoint reference in the resource for later use
        builder.Resource.ApiEndpointReference = endpointReference;

        // Add a reference to ensure the dependency is tracked
        builder.WithReference(endpointReference);

        return builder;
    }
}