 
namespace IvoryPayAssessment.Application.Helpers
{
    public static class EncDecHelper
    {
        public static bool AllowedEmails(string customerEmail)
        {
            var strings = customerEmail.Split('@');
            var str = new string[] { "gmail.com", "outlook.com", "yahoo.com", "icloud.com", "hotmail.com" };
            return str.Contains(strings[strings.Length - 1]);
        }

        public static string GenerateNumericKey(int size)
        {
            char[] chars = "1234567890875643".ToCharArray();
            byte[] data = new byte[size];

            //using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            // { crypto.GetBytes(data); }

            using (var crypto = RandomNumberGenerator.Create())
            { crypto.GetBytes(data); }
            StringBuilder result = new StringBuilder(size);


            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


        public static string GenerateAppId(int size)
        {
            char[] chars = "1234567890875643ABCDEFGHIJKLMNOPQRSTUVWXYZ0987654321ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            byte[] data = new byte[size];

            //using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            // { crypto.GetBytes(data); }

            using (var crypto = RandomNumberGenerator.Create())
            { crypto.GetBytes(data); }
            StringBuilder result = new StringBuilder(size);


            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public    static string EncryptText<T>(T code)
        {
           var key= Convert.ToString(code);
            var data = BCrypt.Net.BCrypt.EnhancedHashPassword(key,HashType.SHA512);
            return   data;
        }
        public static bool DecryptText<T>(string cipherText, T code)
        {   var key= Convert.ToString(code);

            var data = BCrypt.Net.BCrypt.EnhancedVerify(key,cipherText, HashType.SHA512);
            return data;
        }

        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt<T>(T data,IConfiguration conf)
        {
            var passPhrase = conf.GetValue<string>("SystemSettings:PAsswordKey");
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var serialized=JsonConvert.SerializeObject(data);
            var plainTextBytes = Encoding.UTF8.GetBytes(serialized);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static T Decrypt<T>(string cipherText, IConfiguration conf)
        {
           string passPhrase= conf.GetValue<string>("SystemSettings:PAsswordKey");
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
         
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
          
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
           
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                            {
                                var raed= streamReader.ReadToEnd();
                                var converted=JsonConvert.DeserializeObject<T>(raed);
                                return converted;
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}

