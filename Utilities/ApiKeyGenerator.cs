namespace Utilities;

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class ApiKeyGenerator
{
    public static string GenerateApiKey()
    {
        // Your implementation for generating a random string (e.g., using GUID)
        return Guid.NewGuid().ToString("N");
    }

    public static string HashApiKey(string apiKey)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(apiKey));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }
    }
}
