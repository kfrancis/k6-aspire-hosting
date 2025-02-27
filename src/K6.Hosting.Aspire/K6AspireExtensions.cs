using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using K6.Hosting.Core;

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
        var scriptDir = Path.GetFullPath(options.ScriptDirectory) ?? "";
        var scriptFileName = Path.GetFileName(options.ScriptFileName);

        var resource = new K6Resource(name, options);

        // let's use the k6 docker image here
        return builder.AddResource(resource)
            .WithImage(options.ImageConfig.Image)
            .WithImageRegistry(options.ImageConfig.Registry)
            .WithImageTag(options.ImageConfig.Tag)
            .WithEnvironment("K6_INSECURE_SKIP_TLS_VERIFY", "true")
            .WithEndpoint(0, K6Port, name: "k6-api")
            .WithBindMount(scriptDir, "/scripts")
            .WithArgs("run", $"/scripts/{scriptFileName}")
            .WithExplicitStart();
    }

    /// <summary>
    ///     Adds a k6 container resource to the <see cref="IDistributedApplicationBuilder" />.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="IDistributedApplicationBuilder" /> to which the k6 container resource will be
    /// </param>
    /// <param name="apiProject">
    ///     The <see cref="IResourceBuilder{ProjectResource}" /> that represents the project to undergo k6 testing.
    /// </param>
    /// <returns>
    ///     A reference to the <see cref="IResourceBuilder{K6Resource}" /> for further resource configuration.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no endpoints are found for the API project.
    /// </exception>
    public static IResourceBuilder<K6Resource> WithApiEndpoint(this IResourceBuilder<K6Resource> builder,
        IResourceBuilder<ProjectResource> apiProject)
    {
        // Get all endpoint annotations for the resource
        var endpointAnnotations = apiProject.Resource.Annotations
            .OfType<EndpointAnnotation>()
            .ToList();

        if (endpointAnnotations.Count == 0)
        {
            throw new InvalidOperationException(
                $"No endpoints found for resource '{apiProject.Resource.Name}'. " +
                "Make sure the API has at least one endpoint defined.");
        }

        // Try to find an HTTPS endpoint first, then HTTP, then any endpoint
        var endpointAnnotation = endpointAnnotations
                                     .FirstOrDefault(e =>
                                         e.UriScheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                                 ?? endpointAnnotations
                                     .FirstOrDefault(e =>
                                         e.UriScheme.Equals("http", StringComparison.OrdinalIgnoreCase))
                                 ?? endpointAnnotations.First();

        // Get the endpoint reference
        var endpointReference = new EndpointReference(apiProject.Resource, endpointAnnotation.Name);

        // Store the endpoint reference in the resource for later use
        builder.Resource.ApiEndpointReference = endpointReference;

        // Add a reference to ensure the dependency is tracked
        builder.WithReference(apiProject);

        // Automatically add the API URL as an environment variable APP_HOST
        // Add environment variables for k6 script to use
        builder.WithEnvironment("APP_HOST", endpointAnnotation.TargetHost + $":{endpointAnnotation.Port}");
        builder.WithEnvironment("APP_HOST2", "host.docker.internal" + $":{endpointAnnotation.Port}");
        builder.WithEnvironment("APP_ENDPOINT_SCHEME", endpointAnnotation.UriScheme);

        return builder;
    }
}
