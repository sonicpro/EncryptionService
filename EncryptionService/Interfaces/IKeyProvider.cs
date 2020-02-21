namespace Encryption.Interfaces
{
    public interface IKeyProvider
    {
        string Key { get; }

        void RotateKey();
    }
}
