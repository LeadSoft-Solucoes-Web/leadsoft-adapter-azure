using Microsoft.Extensions.DependencyInjection;

namespace LeadSoft.Adapter.Azure.OCR
{
    public static class Register
    {
        public static void AddAzureSpeechToTextService(this IServiceCollection services)
        {
            //services.AddSingleton<,>();
        }

        //public static void AddAzureSpeechToTextService(this IServiceCollection services, AssumeRoleRequest? assumeRole = null, ILogger<AwsSecretManager>? logger = null)
        //{
        //    services.AddSingleton<>(awsSecretManager => new AwsSecretManager(assumeRole, logger));
        //}
    }
}
