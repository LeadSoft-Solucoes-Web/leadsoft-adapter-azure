using LeadSoft.Adapter.Azure.EntraID.Contracts;

namespace LeadSoft.Adapter.Azure.EntraID
{
    /// <summary>
    /// Defines methods for performing Azure Single Sign-On (SSO) operations, including group membership management and
    /// user authentication, using OAuth and Azure Entra ID.
    /// </summary>
    /// <remarks>Implementations of this interface provide asynchronous operations for integrating with Azure
    /// SSO scenarios, such as adding users to groups and retrieving user profiles. All methods are asynchronous and may
    /// require appropriate Azure permissions. The interface inherits from IDisposable, indicating that implementations
    /// may hold resources that should be released when no longer needed.</remarks>
    public interface IAzureSSO : IDisposable
    {
        /// <summary>
        /// Asynchronously adds one or more members to the group using their email addresses.
        /// </summary>
        /// <param name="memberEmails">An array of email addresses representing the users to add as group members. Each email address must be a
        /// valid, non-empty string.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all members
        /// were successfully added; otherwise, <see langword="false"/>.</returns>
        Task<bool> AddGroupMembersAsync(params string[] memberEmails);

        /// <summary>
        /// Initiates an OAuth single sign-on (SSO) authentication process using the specified user code.
        /// </summary>
        /// <param name="oAuthUserCode">The user code received from the OAuth provider to initiate the SSO authentication. Cannot be null or empty.</param>
        /// <param name="relogin">true to force a new authentication session even if a valid session exists; otherwise, false.</param>
        /// <param name="getPicture">true to include the user's profile picture in the response if available; otherwise, false.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DTOAzureEntraIDSSOResponse
        /// object with the authentication result and user information.</returns>
        Task<DTOAzureEntraIDSSOResponse> GetOAuthSSOAsync(string oAuthUserCode, bool relogin = false, bool getPicture = false);

        /// <summary>
        /// Asynchronously retrieves the user profile associated with the specified email address.
        /// </summary>
        /// <param name="userEmail">The email address of the user whose profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a
        /// DTOAzureEntraIDUserProfileResponse object with the user's profile information.</returns>
        Task<DTOAzureEntraIDUserProfileResponse> GetUserProfileAsync(string userEmail);

        /// <summary>
        /// Asynchronously retrieves the current environment name or identifier.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// current environment name or identifier.</returns>
        Task<string> GetEnvironmentAsync();
    }
}
