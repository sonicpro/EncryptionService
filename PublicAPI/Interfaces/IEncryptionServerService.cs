using System.Threading.Tasks;

namespace PublicAPI.Interfaces
{
    public interface IEncryptionServerService
    {
        Task<string> Encrypt(string data);

        Task<string> Decrypt(string encryptedData);
    }
}
