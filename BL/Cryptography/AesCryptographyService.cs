using System.Security.Cryptography;
using System.Text;

namespace BL.Cryptography
{
    public class AesCryptographyService : IAesCryptographyService
    {
        private readonly byte[] IV =
            {
                0x36, 0x02, 0x03, 0x04, 0x05, 0x93, 0x07, 0x34,
                0x09, 0x78, 0x11, 0x99, 0xd1, 0x14, 0x98, 0x16
            };

        private readonly string passPhase = "52_^QcZ{{=6=CF?7";

        public async Task<byte[]> EncryptAsync(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKey(passPhase);
            aes.IV = IV;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(plainText));
            await cryptoStream.FlushFinalBlockAsync();
            return output.ToArray();
        }

        public async Task<string> DecryptAsync(byte[] encrypted)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKey(passPhase);
            aes.IV = IV;
            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }

        private byte[] DeriveKey(string passphrase)
        {
            byte[] emptySalt = Array.Empty<byte>();
            int iterations = 1000;
            int desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(passphrase),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }
    }
}