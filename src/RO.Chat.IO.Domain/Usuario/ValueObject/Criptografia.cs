using System;
using System.Security.Cryptography;
using System.Text;

namespace RO.Chat.IO.Domain.Usuario.ValueObject
{
    public class Criptografia
    {
        public static string GerarHash(string input)
        {
            string salt = "3EF49399-AE1D-49A7-A579-F77FA8EA45B1";
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
