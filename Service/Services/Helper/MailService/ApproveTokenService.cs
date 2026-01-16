using System.Security.Cryptography;

namespace Service.Services.Helper.MailService;

public class ApproveTokenService
{
    private static readonly string EncryptionKey = "Lya+xesyf6A5T7OS4W/DSmeCjOa3pwI5E8ODgoKSPnw=";
    public static string GenerateToken(string text)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(EncryptionKey);
            aesAlg.IV = new byte[16];

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                }
                byte[] encryptedBytes = msEncrypt.ToArray();
                return Base64UrlEncode(encryptedBytes);
            }
        }
    }

    public static string DecryptToken(string encryptedText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(EncryptionKey);
            aesAlg.IV = new byte[16];

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] encryptedBytes = Base64UrlDecode(encryptedText);

            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
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


    public static bool IsEncryptedTextValid(string encryptedText)
    {
        try
        {
            DecryptToken(encryptedText);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public static byte[] Base64UrlDecode(string input)
    {
        input = input.Replace('-', '+').Replace('_', '/');
        int padding = (4 - input.Length % 4) % 4;
        input = input.PadRight(input.Length + padding, '=');
        return Convert.FromBase64String(input);
    }

    public static string GenerateRandomKey(int keyLength)
    {
        using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[keyLength];
            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes, Base64FormattingOptions.None);
        }
    }

    // public static string GenerateRandomKey(int keyLength)
    // {
    //     using (var randomNumberGenerator = new RNGCryptoServiceProvider())
    //     {
    //         var randomBytes = new byte[keyLength];
    //         randomNumberGenerator.GetBytes(randomBytes);
    //         return Convert.ToBase64String(randomBytes);
    //     }
    // }

}