using PublicAPI.Interfaces;
using PublicAPI.Shared;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class AuthorizationServerService : IAuthorizationServerService
    {
        public async Task<AccessToken> AcquireToken(string authGrant = null, string grantType = null, string clientId = null, string clientSecret = null)
        {
            return await Task.FromResult(new AccessToken());
        }
    }
}
