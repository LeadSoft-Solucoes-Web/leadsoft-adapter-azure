using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LeadSoft.Adapter.Azure.EntraID.Contracts.AzureEntraID
{
    [Serializable]
    [DataContract]
    public partial record AuthData
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("access_token")]
        public string AccessToken { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("scope")]
        public string Scope { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("instance_url")]
        public string InstaceUrl { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("token_type")]
        public string TokenType { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("error")]
        public string Error { get; private set; } = string.Empty;

        [DataMember]
        [JsonProperty("error_description")]
        public string ErrorDescription { get; private set; } = string.Empty;
    }
}
