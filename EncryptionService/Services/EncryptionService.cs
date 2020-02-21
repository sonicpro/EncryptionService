using Encryption.Interfaces;
using System.Threading.Tasks;

namespace Encryption.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IKeyProvider keyProvider;
        private readonly IEncryptionHelperService encryptionHelper;

        public EncryptionService(IKeyProvider provider, IEncryptionHelperService helper)
        {
            keyProvider = provider;
            encryptionHelper = helper;
        }

        public async Task<string> Decrypt(byte[] data)
        {
            return await Task.FromResult(encryptionHelper.Decrypt(data, keyProvider.Key));
        }

        public async Task<byte[]> Encrypt(string textData)
        {
            return await Task.FromResult(encryptionHelper.Encrypt(textData, keyProvider.Key));
        }

        public async Task RotateKey()
        {
            await Task.Run(() => keyProvider.RotateKey());
        }
    }
}
