using System.Security.Cryptography;
using System.Text;

namespace Service.Utils.Security;

 public class SecurityHelper
    {

        public SecurityHelper() { }

        public static byte[] GenerateSalt(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[length];
                rng.GetBytes(salt);
                return salt;
            }
        }
        public static byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password);
                var saltedPasswordWithSalt = new byte[saltedPassword.Length];

                Buffer.BlockCopy(saltedPassword, 0, saltedPasswordWithSalt, 0, saltedPassword.Length);
                //Buffer.BlockCopy(salt, 0, saltedPasswordWithSalt, saltedPassword.Length);

                return sha256.ComputeHash(saltedPasswordWithSalt);
            }
        }
        public static string ByteArrayToString(byte[] byteArray)
        {
            var sb = new StringBuilder();
            foreach (var b in byteArray)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        public void SavePassword(string username, string password)
        {
            // Generate a salt
            // var salt = GenerateSalt(16);

            // Hash the password with the salt
            var hashedPassword = HashPassword(password);

            // Convert salt and hashed password to strings
            //var saltString = ByteArrayToString(salt);
            var hashedPasswordString = ByteArrayToString(hashedPassword);
            // Save username, hashed password and salt to database

        }
        public bool VerifyPassword(string username, string inputPassword)
        {
            // Retrieve the stored salt and hashed password from the database
            // var user = GetUserByUsername(username); // Implement this method to get user data
            var salt = StringToByteArray("9649b03edeaac8ff5a3570c7320c00dc");
            var storedHashedPassword = StringToByteArray("8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918");

            // Hash the input password with the retrieved salt
            var hashedInputPassword = HashPassword(inputPassword);
            if (hashedInputPassword.SequenceEqual(storedHashedPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
            // Compare the hashed input password with the stored hashed password
            // return hashedInputPassword.SequenceEqual(storedHashedPassword);
        }
        public static byte[] StringToByteArray(string hex)
        {
            var byteArray = new byte[hex.Length / 2];
            for (var i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return byteArray;
        }

        public static string ByteArrayToHexString(byte[] byteArray)
        {
            var sb = new StringBuilder(byteArray.Length * 2);
            foreach (var b in byteArray)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }