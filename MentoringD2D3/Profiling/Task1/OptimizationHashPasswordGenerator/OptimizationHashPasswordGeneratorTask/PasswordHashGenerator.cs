using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OptimizationHashPasswordGeneratorTask
{
    public class PasswordHashGenerator
    {
        public  string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public  string GeneratePasswordHashUsingSaltOptimized_v1(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSaltOptimized_v2(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSaltOptimized_v3(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSaltOptimized_v3_1(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            byte[] hashBytes = new byte[36];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(pbkdf2.GetBytes(20), 0, hashBytes, 16, 20);

            //int count = Array.IndexOf<byte>(hashBytes, 0, 0);
            //if (count < 0) count = hashBytes.Length;
            //return Encoding.ASCII.GetString(hashBytes, 0, count);

            return Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
        }

        public string GeneratePasswordHashUsingSaltOptimized_v3_2(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            
            var hashBytes = salt.Concat(pbkdf2.GetBytes(20));

            //int count = Array.IndexOf<byte>(hashBytes, 0, 0);
            //if (count < 0) count = hashBytes.Length;
            //return Encoding.ASCII.GetString(hashBytes, 0, count);

            return Convert.ToBase64String(hashBytes.ToArray(36), 0, 36, Base64FormattingOptions.None);
        }

        public string GeneratePasswordHashUsingSaltOptimized_v4(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
            return passwordHash;
        }
    }
}
