using System.Security.Cryptography;
using System.Text;
namespace PersonalFolder;
public static class HashingHelper
{
    public static string ReadHashFile(string path)
    {
        return File.ReadAllText(path);
    }
    public static void Sha256EncryptToFile(string phrase, string path)
    {
        File.WriteAllText(path,Sha256Encrypt(phrase));
    }
    public static string Sha256Encrypt(string phrase)
    {
        var encoder = new UTF8Encoding();
        var sha256Hasher = new SHA256Managed();
        var hashedDataBytes = sha256Hasher.ComputeHash(encoder.GetBytes(phrase));
        return Convert.ToBase64String(hashedDataBytes);
    }
}