namespace Encryption
{
    public class EncryptionSettings
    {
        public string Salt { get; set; }

        public int Iterations { get; set; }

        public string[] Keys { get; set; }
    }
}
