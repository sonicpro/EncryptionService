using Encryption.Interfaces;
using Microsoft.Extensions.Options;

namespace Encryption.Services
{
    public class KeyProvider : IKeyProvider
    {
        private readonly string[] keys;
        private int currentKeyIndex = 0;

        public KeyProvider(IOptions<EncryptionSettings> settings)
        {
            keys = settings.Value.Keys;
        }

        public string Key => keys[currentKeyIndex];

        public void RotateKey()
        {
            currentKeyIndex++;
        }
    }
}
