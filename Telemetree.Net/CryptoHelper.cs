using System.Security.Cryptography;

namespace Telemetree.Net
{
    internal static class CryptoHelper
    {
        public static string RsaEncrypt(string publicKeyX509PEM, byte[] message)
        {
            using (var rsa = RSA.Create())
            {
                var base64Key = ConvertX509PemToDer(publicKeyX509PEM);
                rsa.ImportRSAPublicKey(base64Key, out var bytesRead);
                var encryptedBytes = rsa.Encrypt(message, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static (byte[] Key, byte[] IV) GenerateAesKeyIv()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.GenerateKey();
                aes.GenerateIV();
                return (aes.Key, aes.IV);
            }
        }

        // Function to encrypt data using AES
        public static string AesEncrypt(byte[] key, byte[] iv, string message)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(message);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        private static byte[] ConvertX509PemToDer(string pemContents)
        {
            var start = "-----BEGIN RSA PUBLIC KEY-----\n";
            var end = "\n-----END RSA PUBLIC KEY-----";
            var result = pemContents.Substring(start.Length, pemContents.IndexOf(end) - start.Length);
            return Convert.FromBase64String(result);
        }
    }
}
