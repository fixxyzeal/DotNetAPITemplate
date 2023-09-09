namespace BL.Cryptography
{
    public interface IAesCryptographyService
    {
        Task<string> DecryptAsync(byte[] encrypted);

        Task<byte[]> EncryptAsync(string plainText);
    }
}