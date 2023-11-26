using System;

namespace Utilities
{
    public static class ApiKeyGenerator
    {
        public static string GenerateApiKey()
        {
            // Your implementation for generating a random string (e.g., using GUID)
            return Guid.NewGuid().ToString("N");
        }

        public static string HashApiKey(string apiKey)
        {
            using (var sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(apiKey));
                return BitConverter.ToString(data).Replace("-", "").ToLower();
            }
        }

        public static void Main(string[] args)
        {
            // Example: Hashing an API key
            string apiKey = "EEEBAA098E8BCE844DC92B9E72C768BE422BDEA9C900A7C53956F1D382C831";
            string hashedApiKey = HashApiKey(apiKey);

            Console.WriteLine("Original API Key: " + apiKey);
            Console.WriteLine("Hashed API Key: " + hashedApiKey);
        }
    }
}