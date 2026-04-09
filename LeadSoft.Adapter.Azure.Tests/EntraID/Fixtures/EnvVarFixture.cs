namespace LeadSoft.Adapter.Azure.Tests.EntraID.Fixtures
{
    /// <summary>
    /// Provides a test fixture that sets and clears environment variables commonly required for development and integration
    /// testing scenarios.
    /// </summary>
    /// <remarks>Use this class to ensure a consistent set of environment variables is available during test
    /// execution. When an instance is created, it sets predefined values for several environment variables related to
    /// ASP.NET Core, AWS Secrets Manager, and Azure AD authentication. Upon disposal, these environment variables are
    /// removed from the current process. This fixture is intended for use in automated tests that depend on these
    /// environment variables being present and should not be used in production code.</remarks>
    public class EnvVarFixture : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the EnvVarFixture class and sets environment variables required for development and
        /// integration testing.
        /// </summary>
        /// <remarks>This constructor sets several environment variables commonly used for ASP.NET Core and cloud service
        /// authentication to predefined values. It is intended to provide a consistent environment for tests that depend on
        /// these variables. Existing values for these environment variables will be overwritten when an instance of this
        /// fixture is created.</remarks>
        public EnvVarFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ARN", "");
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_NAME", "");
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_REGION", "");
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ROLE_ARN", "");
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ROLE_SESSION_NAME", "");
            Environment.SetEnvironmentVariable("AZURE_AD_CLIENT_ID", "");
            Environment.SetEnvironmentVariable("AZURE_AD_TENANT_ID", "");
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_ID", "");
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_VALUE", "");
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_REDIRECT_URL", "");
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_CLIENT_ID", "");
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_CLIENT_SECRET", "");
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_GROUP_ID", "");
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_TENANT_ID", "");
        }

        /// <summary>
        /// Releases resources used by the object and clears related environment variables.
        /// </summary>
        /// <remarks>Call this method when the object is no longer needed to remove environment variables
        /// related to ASP.NET Core, AWS Secrets Manager, and Azure AD configuration. After calling this method, the
        /// affected environment variables will be unset for the current process.</remarks>
        public void Dispose()
        {
            // Cleanup if needed
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ARN", null);
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_NAME", null);
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_REGION", null);
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ROLE_ARN", null);
            Environment.SetEnvironmentVariable("AWS_SECRETS_MANAGER_ROLE_SESSION_NAME", null);
            Environment.SetEnvironmentVariable("AZURE_AD_CLIENT_ID", null);
            Environment.SetEnvironmentVariable("AZURE_AD_TENANT_ID", null);
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_ID", null);
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_VALUE", null);
            Environment.SetEnvironmentVariable("AZURE_AD_AUTH_TOKEN_REDIRECT_URL", null);
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_CLIENT_ID", null);
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_CLIENT_SECRET", null);
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_GROUP_ID", null);
            Environment.SetEnvironmentVariable("AZURE_AD_GROUP_MEMBERS_TENANT_ID", null);
        }
    }
}
