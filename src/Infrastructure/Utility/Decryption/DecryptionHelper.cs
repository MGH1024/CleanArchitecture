﻿using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Decryption
{
    public static class DecryptionHelper
    {
        private static string EncryptionKey => "CleanArch"; //can be fetched from environment variables

        public static string Decrypt(string cipherText)
        {
            try
            {
                var decrypted = Protector.Decrypt(cipherText, EncryptionKey);
                return decrypted;
            }
            catch (Exception ex)
            {
                Log.Error("Error while decoding encrypted sections in appConfig with message {0}" +
                    ", Going to use the plain text.", ex.Message);
                return cipherText;
            }

        }
    }

    public static class Protector
    {
        public static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
            return clearText;
        }
        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
            return cipherText;
        }
    }
}
