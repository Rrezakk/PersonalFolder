using System.Security.Cryptography;
using System.Text;

namespace PersonalFolder;
public class EncryptionHelper
{
    private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.CBC;
    private const string SaltValue = "SodiumChloride";
    private const int KeySize = 32;
    public readonly string Key4;
    public EncryptionHelper(string key4)
    {
        Key4 = key4;
    }
    private static string ConcatChars(IEnumerable<char> chars)
    {
        var sb = new StringBuilder();
        foreach (var c in chars)
            sb.Append(c);
        return sb.ToString();
    }
    public byte[] EncryptBytes(byte[] inputBytes)
    {
        return EncryptBytes(inputBytes, Key4);
    }
    public static byte[] EncryptBytes(byte[] inputBytes,string pass)
    {
        var rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode;
        var salt = Encoding.ASCII.GetBytes(SaltValue);
        var password = new PasswordDeriveBytes(pass, salt, "SHA1", 2);

        var encryptor = rijndaelCipher.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
        cryptoStream.FlushFinalBlock();
        return memoryStream.ToArray();
    }
    public byte[] DecryptBytes(byte[] encryptedBytes)
    {
        return DecryptBytes(encryptedBytes, Key4);
    }
    public static byte[] DecryptBytes(byte[] encryptedBytes,string pass)
    {
        var rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode;
        var salt = Encoding.ASCII.GetBytes(SaltValue);
        var password = new PasswordDeriveBytes(pass, salt, "SHA1", 2);
        var decryptor = rijndaelCipher.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));
        using var memoryStream = new MemoryStream(encryptedBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainBytes = new byte[encryptedBytes.Length];
        cryptoStream.Read(plainBytes, 0, plainBytes.Length);
        return plainBytes;
    }
    public string EncryptText(string text)
    {
        return EncryptText(text, Key4);
    }
    public static string EncryptText(string text,string password)
    {
        var inputBytes = text.Select(Convert.ToByte).ToArray();
        var encRes = EncryptBytes(inputBytes,password);
        var l2 = encRes.Select(Convert.ToChar);
        return ConcatChars(l2);
    }
    public string DecryptText(string text)
    {
        return DecryptText(text, Key4);
    }
    public static string DecryptText(string text,string password)
    {
        var inputBytes = text.Select(Convert.ToByte).ToArray();
        var encRes = DecryptBytes(inputBytes,password);
        var l2 = encRes.Select(Convert.ToChar);
        return ConcatChars(l2);
    }
    public void EncryptFile(string filePath)
    {
        var containment = File.ReadAllBytes(filePath);
        var decryptedContainment = EncryptBytes(containment);
        var newFilePath = Program.LockerDirectoryPath + Path.GetFileName(filePath);
        File.WriteAllBytes(newFilePath, decryptedContainment);
    }
    public void DecryptFile(string filePath)
    {
        var containment = File.ReadAllBytes(filePath);
        var decryptedContainment = DecryptBytes(containment);
        var newFilePath = Program.DesktopUnlockedFolderPath + Path.GetFileName(filePath);
        File.WriteAllBytes(newFilePath,decryptedContainment);
    }
}