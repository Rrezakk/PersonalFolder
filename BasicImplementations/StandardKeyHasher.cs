using PersonalFolder.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PersonalFolder.BasicImplementations;

public class StandardKeyHasher:IKeyHasher
{
    public string GetHash(byte[] key)
    {
        return Sha256Encrypt(key);
    }
    private static string Sha256Encrypt(byte[] textBytes)
    {
        var sha256Hasher = new SHA256Managed();
        var hashedDataBytes = sha256Hasher.ComputeHash(textBytes);
        return Convert.ToBase64String(hashedDataBytes);
    }
}
