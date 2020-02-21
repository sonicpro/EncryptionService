using PublicAPI.Shared;
using System.Threading.Tasks;

namespace PublicAPI.Interfaces
{
    public interface IAuthorizationServerService
    {
        // In real application AccessToken provision
        // will be the responsibility of the authorization endpoint party.
        Task<AccessToken> AcquireToken(string authGrant = null, string grantType = null, string clientId = null, string clientSecret = null);
    }
}
