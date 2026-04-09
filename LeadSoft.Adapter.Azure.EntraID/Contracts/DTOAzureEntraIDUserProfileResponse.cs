using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LeadSoft.Adapter.Azure.EntraID.Contracts
{
    [Serializable]
    [DataContract]
    public partial record DTOAzureEntraIDUserProfileResponse(string displayName, string avatarUrl)
    {
        [DataMember]
        public string DisplayName { get; private set; } = displayName;

        [DataMember]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; private set; } = avatarUrl;
    }
}
