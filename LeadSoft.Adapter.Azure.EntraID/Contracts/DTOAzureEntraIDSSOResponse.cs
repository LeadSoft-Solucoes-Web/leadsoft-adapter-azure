
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LeadSoft.Adapter.Azure.EntraID.Contracts
{
    [Serializable]
    [DataContract]
    public partial record DTOAzureEntraIDSSOResponse(string refreshToken, string integrationId)
    {
        [DataMember]
        public string RefreshToken { get; private set; } = refreshToken;

        [DataMember]
        public string IntegrationId { get; private set; } = integrationId;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOAzureEntraIDUserProfileResponse UserProfileResponse { get; private set; } = null;

        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime When { get; } = DateTime.UtcNow;

        public DTOAzureEntraIDSSOResponse SetAvatar(DTOAzureEntraIDUserProfileResponse dto)
        {
            UserProfileResponse = dto;
            return this;
        }
    }
}
