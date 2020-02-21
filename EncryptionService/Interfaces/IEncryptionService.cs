using System.Threading.Tasks;

namespace Encryption.Interfaces
{
    public interface IEncryptionService
    {
        Task<byte[]> Encrypt(string textData);

        Task<string> Decrypt(byte[] data);

        Task RotateKey();
    }
}
