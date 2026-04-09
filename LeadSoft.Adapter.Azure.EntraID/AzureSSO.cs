using LeadSoft.Adapter.Aws.SecretsManager;
using LeadSoft.Common.Library.EnvUtils;
using LeadSoft.Common.Library.Extensions;

namespace LeadSoft.Adapter.Azure.EntraID
{
    public sealed partial class AzureSSO : IAzureSSO
    {
        private readonly IAwsSecretManager _AwsSecretManager;

        private readonly HttpClient _HttpClient;

        public AzureSSO()
        {
            _HttpClient = new()
            {
                BaseAddress = new Uri(AzureSSO_Parameter.OAuth2_Url.Fill(EnvUtil.Get(EnvVariable.TenantId)))
            };
        }

        public AzureSSO(IAwsSecretManager awsSecretManager)
        {
            _AwsSecretManager = awsSecretManager;

            _HttpClient = new()
            {
                BaseAddress = new Uri(AzureSSO_Parameter.OAuth2_Url.Fill(_AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.TenantId)))
            };
        }

        #region [ Dispose pattern ]

        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _HttpClient.Dispose();
                    _AwsSecretManager?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AzureSSO()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
