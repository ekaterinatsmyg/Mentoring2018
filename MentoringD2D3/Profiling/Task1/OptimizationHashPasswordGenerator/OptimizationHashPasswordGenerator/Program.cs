using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationHashPasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = "password";
            byte[] salt = new byte[] {203, 201, 22, 1, 0, 99, 76, 34, 23, 12, 22, 123, 24, 35, 46, 47};
            GeneratePasswordHashUsingSalt(password, salt);
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
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
    }
}
