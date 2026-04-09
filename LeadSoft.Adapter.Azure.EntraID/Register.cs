using Microsoft.Extensions.DependencyInjection;

namespace LeadSoft.Adapter.Azure.EntraID
{
    /// <summary>
    /// Provides extension methods for registering Azure single sign-on (SSO) services with an IServiceCollection for
    /// dependency injection.
    /// </summary>
    /// <remarks>Use the methods in this class to add Azure SSO functionality to an application's service
    /// collection. These methods enable consuming components to receive an IAzureSSO implementation via dependency
    /// injection. Call the appropriate method during application startup to configure the desired service
    /// lifetime.</remarks>
    public static class Register
    {
        /// <summary>
        /// Adds a singleton implementation of the IAzureSSO service to the specified IServiceCollection.
        /// </summary>
        /// <remarks>Call this method during application startup to register Azure single sign-on services
        /// for dependency injection. This enables consuming components to receive an IAzureSSO instance via constructor
        /// injection.</remarks>
        /// <param name="services">The IServiceCollection to which the IAzureSSO singleton service will be added.</param>
        public static void AddSingletonAzureSSO(this IServiceCollection services)
        {
            services.AddSingleton<IAzureSSO, AzureSSO>();
        }

        /// <summary>
        /// Adds the Azure single sign-on (SSO) service to the specified service collection with a scoped lifetime.
        /// </summary>
        /// <remarks>This extension method registers the IAzureSSO implementation for dependency
        /// injection, enabling Azure SSO functionality throughout the application's scope.</remarks>
        /// <param name="services">The service collection to which the Azure SSO service will be added. Cannot be null.</param>
        public static void AddScopedAzureSSO(this IServiceCollection services)
        {
            services.AddSingleton<IAzureSSO, AzureSSO>();
        }
    }
}
