using Encryption.Shared;
using System.Threading.Tasks;

namespace Encryption.Interfaces
{
    public interface ITokenValidator
    {
        Task<AuthResult> ValidateGrant(AccessToken accessToken);
    }
}
