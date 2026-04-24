using Azure.Identity;
using LeadSoft.Adapter.Azure.EntraID.Contracts;
using LeadSoft.Adapter.Azure.EntraID.Contracts.AzureEntraID;
using LeadSoft.Common.GlobalDomain.Entities;
using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.EnvUtils;
using LeadSoft.Common.Library.Exceptions;
using LeadSoft.Common.Library.Extensions;
using LeadSoft.Common.Library.Extensions.Helpers;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;
using System.Text;

namespace LeadSoft.Adapter.Azure.EntraID
{
    public sealed partial class AzureSSO
    {
        public async Task<bool> AddGroupMembersAsync(params string[] memberEmails)
        {
            memberEmails = [.. memberEmails.Where(e => e.IsValidEmail())];

            if (memberEmails.Length == 0)
                return true;

            try
            {
                GraphServiceClient graphClient = new(await GetParameters());

                UserCollectionResponse users = await graphClient.Users.GetAsync(request =>
                                                                      {
                                                                          request.QueryParameters.Filter = $".Filter($\"mail in ({new StringBuilder().AppendJoin(',', memberEmails.Select(e => $"'{e}'")).ToString()})\")";
                                                                          request.QueryParameters.Select = ["displayName", "id"];
                                                                      });

                if (users is null || !users.Value.Any())
                    return true;

                DirectoryObjectCollectionResponse members = await graphClient.Groups[await GetGroupId()].Members
                                                                             .GetAsync(request => request.QueryParameters.Select = ["mail", "id"]);

                if (members is null)
                    return true;

                IEnumerable<string> memberIds = members.Value.Select(m => m.Id);
                IEnumerable<string> pendingUserIds = users.Value.Where(u => !memberIds.Contains(u.Id)).Select(u => u.Id);

                if (!pendingUserIds.Any())
                    return true;

                Group group = new()
                {
                    AdditionalData = new Dictionary<string, object>()
                    {
                        {"members@odata.bind", pendingUserIds.Select(id => AzureSSO_Parameter.MSGraph_DirectoryObjects.Fill(id)).ToList()}
                    }
                };

                await graphClient.Groups[await GetGroupId()].PatchAsync(group);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<DTOAzureEntraIDSSOResponse> GetOAuthSSOAsync(string oAuthUserCode, bool relogin = false, bool getPicture = false)
        {
            HttpResponseMessage response = await _HttpClient.PostAsync(await GetOAuthSSOEndpoint(), await GetOAuthContent(oAuthUserCode, relogin));
            string responseContent = await response.Content.ReadAsStringAsync();

            OAuthTokenData oauthTokenData = !response.Content.Headers.ContentType.MediaType.Equals(Constant.ApplicationJson, StringComparison.OrdinalIgnoreCase)
                                                ? throw new BadRequestAppException($"Unexpected content type: {response.Content.Headers.ContentType}. Content: {responseContent}")
                                                : responseContent.JsonToObject<OAuthTokenData>();

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new BadRequestAppException(oauthTokenData.ErrorDescription);

            if (!getPicture)
                return new DTOAzureEntraIDSSOResponse(oauthTokenData.RefreshToken, oauthTokenData.AccessToken.RetrieveClaimField(AzureSSO_FormContent.UniqueName));

            string integrationId = oauthTokenData.AccessToken.RetrieveClaimField(AzureSSO_FormContent.UniqueName);

            return new DTOAzureEntraIDSSOResponse(oauthTokenData.RefreshToken, integrationId).SetAvatar(await GetUserProfileAsync(integrationId));
        }



        public async Task<DTOAzureEntraIDUserProfileResponse> GetUserProfileAsync(string userEmail)
        {
            string[] scopes = [AzureSSO_Parameter.MSGraph_UserRead];

            DeviceCodeCredentialOptions options = new()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = _AwsSecretManager is null ? EnvUtil.Get(EnvVariable.ClientId) : await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.ClientId),
                TenantId = _AwsSecretManager is null ? EnvUtil.Get(EnvVariable.TenantId) : await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.TenantId),
                DeviceCodeCallback = (code, cancellation) => Task.FromResult(0),
            };

            GraphServiceClient graphClient = new(new DeviceCodeCredential(options), scopes);

            try
            {
                User? user = await graphClient.Users[userEmail].GetAsync(request => request.QueryParameters.Select = [AzureSSO_Parameter.DisplayName]);
                string displayName = user?.DisplayName ?? userEmail;
                string avatarUrl = string.Empty;

                try
                {
                    Stream photo = await graphClient.Users[userEmail].Photo.Content.GetAsync();
                    if (photo != null)
                    {
                        using MemoryStream ms = new();
                        await photo.CopyToAsync(ms);
                        avatarUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
                    }
                }
                catch
                {
                }

                return new(displayName, avatarUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(userEmail, string.Empty);
            }
        }

        public async Task<string> GetEnvironmentAsync()
        {
            if (_AwsSecretManager is not null)
                return $"Aws Secrets Manager: {await _AwsSecretManager.GetSecretValueAsync(EnvUtil.AspNet)}";

            return $"Environment Variable: {EnvUtil.Get(EnvUtil.AspNet)}";
        }

        #region [ Private methods ]

        private async Task<string> GetOAuthSSOEndpoint()
            => _AwsSecretManager is not null
                ? AzureSSO_Parameter.OAuth2_Url.Fill(await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.TenantId))
                : AzureSSO_Parameter.OAuth2_Url.Fill(EnvUtil.Get(EnvVariable.TenantId));

        private async Task<FormUrlEncodedContent> GetOAuthContent(string oAuthToken, bool reLogin)
        {
            if (_AwsSecretManager is not null)
                return new(
                [
                    new KeyValuePair<string, string>(AzureSSO_FormContent.ClientId, await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.ClientId)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.ClientSecret, await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.AuthToken_ClientSecretValue)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.RedirectUri, await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.AuthToken_RedirectURL)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.Scope, AzureSSO_Parameter.MSGraph_MailRead),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.GrantType, reLogin ? AzureSSO_Parameter.GrantType_RefreshToken : AzureSSO_Parameter.GrantType_AuthorizationCode),
                    reLogin
                        ? new KeyValuePair<string, string>(AzureSSO_FormContent.RefreshToken, oAuthToken)
                        : new KeyValuePair<string, string>(AzureSSO_FormContent.Code, oAuthToken)
                ]);

            return new(
                [
                    new KeyValuePair<string, string>(AzureSSO_FormContent.ClientId, EnvUtil.Get(EnvVariable.ClientId)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.ClientSecret, EnvUtil.Get(EnvVariable.AuthToken_ClientSecretValue)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.RedirectUri, EnvUtil.Get(EnvVariable.AuthToken_RedirectURL)),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.Scope, AzureSSO_Parameter.MSGraph_MailRead),
                    new KeyValuePair<string, string>(AzureSSO_FormContent.GrantType, reLogin ? AzureSSO_Parameter.GrantType_RefreshToken : AzureSSO_Parameter.GrantType_AuthorizationCode),
                    reLogin
                        ? new KeyValuePair<string, string>(AzureSSO_FormContent.RefreshToken, oAuthToken)
                        : new KeyValuePair<string, string>(AzureSSO_FormContent.Code, oAuthToken)
                ]);
        }

        private async Task<ClientSecretCredential> GetParameters()
        {
            TokenCredentialOptions options = new()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            if (_AwsSecretManager is not null)
                return new(await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.GroupMembers_TenantId),
                           await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.GroupMembers_ClientId),
                           await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.GroupMembers_ClientSecret),
                           options);

            return new(EnvUtil.Get(EnvVariable.GroupMembers_TenantId),
                       EnvUtil.Get(EnvVariable.GroupMembers_ClientId),
                       EnvUtil.Get(EnvVariable.GroupMembers_ClientSecret),
                       options);
        }

        private async Task<string> GetGroupId()
            => _AwsSecretManager is null
                ? EnvUtil.Get(EnvVariable.GroupMembers_GroupId)
                : await _AwsSecretManager.GetSecretValueAsync(AwsSecretsKey.GroupMembers_GroupId);

        #endregion
    }
}
