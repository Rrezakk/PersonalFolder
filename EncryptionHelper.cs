using System.Security.Cryptography;
using System.Text;
namespace PersonalFolder;
public class EncryptionHelper
{
    private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.CBC;
    private const string SaltValue = "SodiumChloride";
    public readonly string Key4;
    private static readonly Dictionary<string, string> map = new()
    {
        {"<", "($lessthan$)"},
        {">", "($greater than$)"},
        {":", "($colon$)"},
        {"\"", "($double quote$)"},
        {"/", "($forward slash$)"},
        {"\\", "($back slash$)"},
        {"|", "($vertical bar or pipe$)"},
        {"?", "($question mark$)"},
        {"*", "($asterisk$)"}
    };
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
    private static string ReplaceUnavailable(string str)
    {
        return map.Aggregate(str, (current, pair) => current.Replace(pair.Key, pair.Value));
    }
    private static string ReplaceBackUnavailable(string str)
    {
        return map.Aggregate(str, (current, pair) => current.Replace(pair.Value, pair.Key));
    }
    public void EncryptFile(string filePath)
    {
        //filename encryption?
        //filename ascii encryption base64 replacing
        
        var filename = Path.GetFileNameWithoutExtension(filePath);
        var filename2 = Encoding.ASCII.GetBytes(filename);
        var encrypytedFileNameWOExtension = EncryptBytes(filename2);
        var encrypytedFileNameWOExtension2 = Convert.ToBase64String(encrypytedFileNameWOExtension);//string.Join('_',encrypytedFileNameWOExtension);
        var finalFileName = ReplaceUnavailable(encrypytedFileNameWOExtension2);

        var extension = Path.GetExtension(filePath);
        var encrypytedFileName = finalFileName + extension;

        Console.WriteLine(filename);
        Console.WriteLine(string.Join(' ', filename2));
        Console.WriteLine(string.Join(' ', encrypytedFileNameWOExtension));
        Console.WriteLine(encrypytedFileNameWOExtension2);
        Console.WriteLine(finalFileName);
        
        var containment = File.ReadAllBytes(filePath);
        var encryptedContainment = EncryptBytes(containment);
        var newFilePath = Program.LockerDirectoryPath + encrypytedFileName;
        File.WriteAllBytes(newFilePath, encryptedContainment);
    }
    public void DecryptFile(string filePath)
    {
        //filename decryption?
        //filename replacing base64 encryption ascii
        if (!Directory.Exists(Program.DesktopUnlockedFolderPath))
            Directory.CreateDirectory(Program.DesktopUnlockedFolderPath);

        var filename = Path.GetFileNameWithoutExtension(filePath);
        var f2name = ReplaceBackUnavailable(filename);
        var f3name = Convert.FromBase64String(f2name);//f2name.Split('_').Select(x=>(byte)int.Parse(x)).ToArray();
        var f4name = DecryptBytes(f3name);
        f4name=f4name.TrimTailingZeros();
        var decrypytedFileNameWOExtension = Encoding.ASCII.GetString(f4name);



        Console.WriteLine(decrypytedFileNameWOExtension);
        Console.WriteLine(filename);
        Console.WriteLine(f2name);
        Console.WriteLine(string.Join(' ', f3name));
        Console.WriteLine(string.Join(' ', f4name));

        var extension = Path.GetExtension(filePath);
        var decrypytedFileName = decrypytedFileNameWOExtension + extension;

        var containment = File.ReadAllBytes(filePath);
        var decryptedContainment = DecryptBytes(containment);
        var newFilePath = Program.DesktopUnlockedFolderPath + decrypytedFileName;
        File.WriteAllBytes(newFilePath,decryptedContainment);
    }
}