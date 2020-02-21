using Encryption.Shared;

namespace Encryption
{
    public class AuthResult
    {
        public AccessToken Token { get; set; }

        public AuthResultType Type { get; set; }
    }
}
