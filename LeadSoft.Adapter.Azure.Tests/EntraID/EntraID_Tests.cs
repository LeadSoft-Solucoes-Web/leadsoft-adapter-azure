using LeadSoft.Adapter.Aws.SecretsManager;
using LeadSoft.Adapter.Azure.EntraID;
using LeadSoft.Adapter.Azure.EntraID.Contracts;
using LeadSoft.Adapter.Azure.Tests.EntraID.Fixtures;
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
            //AssumeRoleRequest assumeRoleRequest = new()
            //{
            //    RoleArn = EnvUtil.Get("AWS_SECRETS_MANAGER_ROLE_ARN"),
            //    RoleSessionName = EnvUtil.Get("AWS_SECRETS_MANAGER_ROLE_SESSION_NAME"),
            //};

            //_AwsSecretsManager = new AwsSecretManager(assumeRoleRequest, _Logger);
        }

        [Theory]
        [InlineData("1.AbcAGuhY74WHPE-MHVfLZ4kD0bXvPrWbMWlCma9kIufph_4JAIm3AA.BQABBAIAAAADAOz_BQD0_0V2b1N0c0FydGlmYWN0cwIAAAAAAI3jyyAtcNJqx2QPS9Yl0IH5fu9xXgRt7oFEVDsBOF9tHoxwG2pQ2PgQyfZYJa0O4XMIT0i7m6z98fGDhO4xRpq0cXoz5-ZbX9j0J_yhg7whfTZWbQuBINh4x0P5VuTiYb_5cJCU_pVpEU4LkgrZh88O0p43ABzoLBQLbj9cC4TDrnV4KjNvNAUvIrwvbJFDARIrwA3BeXNVUK-H8mjBtet7x4boBO_xnqkBLuQvGO4pAF7hCInXiooaXjSZhAr99ZyJbjPh2s9qgluLvGsj8QUGnKEgS5zU2BiTCi3o1hkyDyXGox9kTx38HdTQNprcNtGsZipJbB3BP48o8sJpbDkbs3Y3tD1qwFE423PTTKcR2GMroIiWFR9hOHZ907p_sK2Hf1K0f_CmgAH8Rk90dF66eyCJxtN2TXZ0Ft7RUPCv6SjpMeB8gTaPlTncZSkmqIUeH6SAw0N6jmjh-6bfu4uEs1tKA-MkgyAyGqa5Pm8G53P4jx5xKekX2p5BVdmRSdZAzZnc-hkStMb23pihkYBk3af6a0YtmgHOoKRIbwGZU394sofeQT-m4kjSmJ1pbu-sNRMK8sCE9-7WSDIsxWmlR1abqgtvq612giRTPo91YpUEQSRqd3XuswGJT8IEpxowfzbXVZAPbKnOrLhVkuE4KH63KccZMWttVMmQ9J5S94H-zWUWrj1w5pA8lJPQ7FbADVWnm1QeL3uAijWUU0nWe9faqgM0v0ouwEAp-TjD9gtvZvUR-D7TMvDeXtECTCiI_tfpKchPIFBOIR_UcEAGJJ6un1WOw6H_99Op3CiBxDGHG2aGAJ2ZAh8icw3e5dnavPrBmKrWRI0z3oCrjJZfzNPgFW-pauG1Fjfu2RF34hTj7YPzUyKs41i_NAOixYjdrNKVe-IA-5lUI9FB8x83QL46AMupUd73fjEoSybAQyrEkBH-OniijmUua-c7qWEZ756t_t24-wUco4Ei_lXv3NvOsNgU6aPpz9Pt3NiD_1M274KCdA2ugTFrTQr9WrRP9hum5-YepByyA_9ByiYnGrAzQlaKJXc0mA271lPc9P2TKuoadTCzTswAMbNYsOOAvnNpmQmH_4PT7vIT-rf3-lCVKZ2LyFwrlSxSoQfKnn8PbXaJYtuLVpp6XGee5QxE_IEiIr6cGabinEfjRDhHgyQivakBJj79Nv0tW-tdTeQHjrJkjNLCcyt3g22nQnqcQfjSEW1SzrQjdcrvoqwvX0gJBAkPLJFgdHMfxxGQFU-jZoxuC6GerMUlYqijKG3JcEjsSfjMa8ahW9YdmYwj7Qih09B59x-6iz4khjvqDrbBKbiPzTbllWZKniZ0W_F1IXHTIgB-NGng_rNARQvVAoeasxnmy7cm6reAk6CWBtHCGJr31HL8PNTCa9VJisVO0Khbv8Z6ropB4T00E2tCrBeQYfLsL2lNcdWiSv3SY633SWEY91PP_42AILsObNkl6tQc69Q2Xj0A3yw6lw8za_QCGg5sLvyh7SfueBPxsarCtkg36cvD1ZAOWBVfTWpQCOJTVit6t8LFA5p_C1g4teh99FB6vFZTD5W4Gp0y5-AUqX8V4Lku1qPaQIWJEP51OTFFWFivecVHMWyVT3lRFHZFSmTTqnGoqeM-jA", false)]
        public async Task GetOAuthSSOAsync(string code, bool avatar)
        {
            IAzureSSO azureSSO = new AzureSSO();

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
