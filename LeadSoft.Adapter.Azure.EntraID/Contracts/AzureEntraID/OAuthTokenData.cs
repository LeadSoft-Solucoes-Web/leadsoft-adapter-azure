using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LeadSoft.Adapter.Azure.EntraID.Contracts.AzureEntraID
{
    [Serializable]
    [DataContract]
    public partial record OAuthTokenData : AuthData
    {
        [DataMember]
        [JsonProperty("id_token")]
        public string IdToken { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("sfdc_community_url")]
        public string SfdcCommunityUrl { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("sfdc_community_id")]
        public string SfdcCommunityId { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("signature")]
        public string Signature { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; private set; }

        [DataMember]
        [JsonProperty("ext_expires_in")]
        public int ExtExpiresIn { get; private set; }
    }
}
