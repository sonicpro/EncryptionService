using Encryption.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Encryption.Helpers
{
    public class EncryptionHelperService : IEncryptionHelperService
    {
        private readonly string salt;

        private readonly int iterations;

        public EncryptionHelperService(IOptions<EncryptionSettings> settings)
        {
            salt = settings.Value.Salt;
            iterations = settings.Value.Iterations;
        }

        public byte[] Encrypt(string data, string password)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException(nameof(data));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            using (var aes = Aes.Create())
            {
                aes.Key = GetEncryptionKey(password);
                aes.IV = GetEncryptionVector(password);

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public string Decrypt(byte[] data, string password)
        {
            if (!data.Any())
            {
                throw new ArgumentException(nameof(data));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            string decryptedString = null;

            using (var aes = Aes.Create())
            {
                aes.Key = GetEncryptionKey(password);
                aes.IV = GetEncryptionVector(password);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedString = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return decryptedString;
            }
        }

        #region Helper Methods

        private byte[] GetEncryptionKey(string password)
        {
            var byteDeriver = new Rfc2898DeriveBytes(password, GetSaltBytes, iterations);
            return byteDeriver.GetBytes(32);
        }

        private byte[] GetEncryptionVector(string password)
        {
            var byteDeriver = new Rfc2898DeriveBytes(password, GetSaltBytes, iterations);
            return byteDeriver.GetBytes(16);
        }

        private byte[] GetSaltBytes => Encoding.UTF8.GetBytes(salt);

        #endregion
    }
}
