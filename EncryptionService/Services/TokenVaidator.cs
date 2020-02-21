using Encryption.Interfaces;
using Encryption.Shared;
using System.Threading.Tasks;

namespace Encryption.Services
{
    public class TokenVaidator : ITokenValidator
    {
        private const string validToken = "4119fa8b6bc53e486b3a318da1eae613d4791158";

        public TokenVaidator()
        {
        }

        public Task<AuthResult> ValidateGrant(AccessToken accessToken)
        {
            if (accessToken.Token == validToken)
            {
                return Task.FromResult(new AuthResult
                {
                    Token = accessToken,
                    Type = AuthResultType.Ok
                });
            }
            return Task.FromResult(new AuthResult
            {
                Token = accessToken,
                Type = AuthResultType.Unauthorized
            });
        }
    }
}
