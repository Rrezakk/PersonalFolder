using PersonalFolder.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PersonalFolder.BasicImplementations;

public class StandardFileNameEncryptor:IFileNameEncryptor
{

    public string EncryptName(string fileName, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        using MemoryStream ms = new();
        ms.Write(aes.IV);
        using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
        {
            cs.Write(Encoding.UTF8.GetBytes(fileName));
        }
        return Convert.ToBase64String(ms.ToArray());
    }
    public string DecryptName(string encryptedFileName, byte[] key)
    {
        using MemoryStream ms = new(Convert.FromBase64String(encryptedFileName));
        var iv = new byte[16];
        var read = ms.Read(iv);
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
        using MemoryStream output = new();
        cs.CopyTo(output);
        return Encoding.UTF8.GetString(output.ToArray());
    }
}
