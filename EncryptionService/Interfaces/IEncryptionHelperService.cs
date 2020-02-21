namespace Encryption.Interfaces
{
    public interface IEncryptionHelperService
    {
        byte[] Encrypt(string data, string key);

        string Decrypt(byte[] data, string key);
    }
}
