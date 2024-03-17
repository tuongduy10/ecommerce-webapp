using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Utilities.Helpers
{
    public static class HashHelper
    {
        private static readonly string secretKey = "8DC7FDE41D3047049243CDDBA1D6EBF9";
        public static string Encrypt(string plainText, string _secretKey = "")
        {
            byte[] encryptedBytes;
            using (Aes aesAlg = Aes.Create())
            {
                string _key = secretKey;
                if (!string.IsNullOrEmpty(_secretKey))
                    _key = _secretKey;
                aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
                aesAlg.IV = new byte[16]; // Initialization Vector

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string Decrypt(string encryptedText, string _secretKey = "")
        {
            byte[] cipherText = Convert.FromBase64String(encryptedText);

            using (Aes aesAlg = Aes.Create())
            {
                string _key = secretKey;
                if (!string.IsNullOrEmpty(_secretKey))
                    _key = _secretKey;
                aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
                aesAlg.IV = new byte[16]; // Initialization Vector

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
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
