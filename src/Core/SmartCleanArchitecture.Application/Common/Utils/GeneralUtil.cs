using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SmartCleanArchitecture.Application.Common.Utils
{
    public static class GeneralUtil
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

        public static string GenerateReference()
        {
            return Guid.NewGuid().ToString();
        }

        public static string TimeStampReference()
        {
            return DateTime.Now.ToString("ddmmyyyyhhmmss");
        }

        public static string SerializeAsJson<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T DeserializeFromJson<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public static string Encryptor(string request, string secretKey)
        {
            #region New AES Encryption

            var requestBytes = Encoding.UTF8.GetBytes(request);
            byte[] ivKey = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = Encoding.UTF8.GetBytes(secretKey);
            aes.IV = ivKey;

            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(requestBytes, 0, requestBytes.Length);
            string result = Convert.ToBase64String(encryptedBytes);
            return result;

            #endregion  
        }


        public static string EncryptorWithSecretKeyAdded(string request, string secretKey)
        {
            #region New AES Encryption

            var requestBytes = Encoding.UTF8.GetBytes(request);
            var secreKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            byte[] ivKey = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = Encoding.UTF8.GetBytes(secretKey);
            aes.IV = ivKey;

            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(requestBytes, 0, requestBytes.Length);
            string result = Convert.ToBase64String(encryptedBytes);
            return result;

            #endregion  
        }

        public static string Decryptor(string encryptedData, string secretKey)
        {
            #region New AES Decryption

            var encryptedTextByte = Convert.FromBase64String(encryptedData);
            byte[] ivKey = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = Encoding.UTF8.GetBytes(secretKey);
            aes.IV = ivKey;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            string result = Encoding.UTF8.GetString(decryptedBytes);
            return result;

           
            #endregion

            
        }

        public static T DecryptRequest<T>(string requestData, string secretKey)
        {
            var decryptedRequest = Decryptor(requestData, secretKey);
            var deserializedRequest = DeserializeFromJson<T>(decryptedRequest);

            return deserializedRequest;
        }

        public static bool IsBase64String(string input)
        {
            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out int bytesParsed);
        }

        public static string UniqueRandomId(int size)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[size];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
