using System.Security.Cryptography;
using System.Text;
using System;

namespace ECommerce.WebApp.Utils
{
    public class EncryptHelper
    {
        private static readonly string secretKey = "0ABE981C-F5BF-434F-A55A-30695C33FA79";
        public static string EncryptString(string str, string key = null)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GenerateKey(secretKey, aesAlg.KeySize / 8);
                aesAlg.IV = new byte[16]; // AES uses a 128-bit block size (16 bytes)

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedData = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(str), 0, str.Length);

                return Convert.ToBase64String(encryptedData);
            }
        }
        public static string DecryptString(string str, string key = null)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GenerateKey(secretKey, aesAlg.KeySize / 8);
                aesAlg.IV = new byte[16]; // AES uses a 128-bit block size (16 bytes)

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = Convert.FromBase64String(str);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        private static byte[] GenerateKey(string secretKey, int keySize)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                byte[] key = new byte[keySize];
                Array.Copy(hash, key, Math.Min(hash.Length, key.Length));
                return key;
            }
        }
    }
}
