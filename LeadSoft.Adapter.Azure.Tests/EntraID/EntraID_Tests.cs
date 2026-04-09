using Amazon.SecurityToken.Model;
using LeadSoft.Adapter.Aws.SecretsManager;
using LeadSoft.Adapter.Azure.EntraID;
using LeadSoft.Adapter.Azure.EntraID.Contracts;
using LeadSoft.Adapter.Azure.Tests.EntraID.Fixtures;
using LeadSoft.Common.Library.EnvUtils;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LeadSoft.Adapter.Azure.Tests.EntraID
{
    [Collection("EnvVar collection")]
    public class EntraID_Tests : IClassFixture<EnvVarFixture>
    {
        private readonly ITestOutputHelper _Output;
        private readonly IAwsSecretManager _AwsSecretsManager;
        private readonly ILogger<AwsSecretManager> _Logger = new Logger<AwsSecretManager>(new LoggerFactory());

        public EntraID_Tests(ITestOutputHelper output, EnvVarFixture envVarFixture)
        {
            _Output = output;

            AssumeRoleRequest assumeRoleRequest = new()
            {
                RoleArn = EnvUtil.Get("AWS_SECRETS_MANAGER_ROLE_ARN"),
                RoleSessionName = EnvUtil.Get("AWS_SECRETS_MANAGER_ROLE_SESSION_NAME"),
            };

            _AwsSecretsManager = new AwsSecretManager(assumeRoleRequest, _Logger);
        }

        [Theory]
        [InlineData("", false)]
        public async Task GetOAuthSSOAsync(string code, bool avatar)
        {
            IAzureSSO azureSSO = new AzureSSO(_AwsSecretsManager);

            DTOAzureEntraIDSSOResponse dtoResponse = await azureSSO.GetOAuthSSOAsync(code, false, avatar);

            _Output.WriteLine($"IntegrationId: {dtoResponse.IntegrationId}");
            _Output.WriteLine($"RefreshToken: {dtoResponse.RefreshToken}");
            _Output.WriteLine($"UserProfile.Name: {dtoResponse.UserProfileResponse?.DisplayName}");
            _Output.WriteLine($"UserProfile.Avatar: {dtoResponse.UserProfileResponse?.AvatarUrl}");
            _Output.WriteLine($"When: {dtoResponse.When}");
        }

        [Theory]
        [InlineData("")]
        public async Task GetOAuthSSOReLoginAsync(string refreshToken)
        {
            IAzureSSO azureSSO = new AzureSSO();

            DTOAzureEntraIDSSOResponse dtoResponse = await azureSSO.GetOAuthSSOAsync(refreshToken, true, true);

            _Output.WriteLine($"IntegrationId: {dtoResponse.IntegrationId}");
            _Output.WriteLine($"RefreshToken: {dtoResponse.RefreshToken}");
            _Output.WriteLine($"UserProfile.Name: {dtoResponse.UserProfileResponse?.DisplayName}");
            _Output.WriteLine($"UserProfile.Avatar: {dtoResponse.UserProfileResponse?.AvatarUrl}");
            _Output.WriteLine($"When: {dtoResponse.When}");
        }

        [Theory]
        [InlineData("lucas.tavares@bp.com")]
        public async Task GetProfilePictureAsync(string email)
        {
            IAzureSSO azureSSO = new AzureSSO();

            DTOAzureEntraIDUserProfileResponse dtoResponse = await azureSSO.GetUserProfileAsync(email);

            _Output.WriteLine($"UserProfile.Name: {dtoResponse.DisplayName}");
            _Output.WriteLine($"UserProfile.Avatar: {dtoResponse.AvatarUrl}");
        }

        [Theory]
        [InlineData("lucas.tavares@bp.com")]
        public async Task AddGroupMembersAsync(string email)
        {
            IAzureSSO azureSSO = new AzureSSO();

            Assert.True(await azureSSO.AddGroupMembersAsync(email));
            _Output.WriteLine($"Group members added for: {email}");
        }
    }
}
