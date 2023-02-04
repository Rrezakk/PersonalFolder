using PersonalFolder.Interfaces;
using System.Security.Cryptography;

namespace PersonalFolder.BasicImplementations;

public class StandardFileEncryptor:IFileEncryptor
{
    public void Encrypt(string sourceFilePath, string destinationFilePath, byte[]? key)
    {
        // Encrypt the source file and write it to the destination file.
        using var sourceStream = File.OpenRead(sourceFilePath);
        using var destinationStream = File.Create(destinationFilePath);
        using var provider = new AesCryptoServiceProvider();
        if (key != null)
        {
            provider.Key = key;
        }

        using var cryptoTransform = provider.CreateEncryptor();
        using var cryptoStream = new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write);
        destinationStream.Write(provider.IV, 0, provider.IV.Length);
        sourceStream.CopyTo(cryptoStream);
        if (key == null)
        {
            Console.WriteLine($"Key was null, generated: {Convert.ToBase64String(provider.Key)}");
        }
    }
    public void Decrypt(string sourceFilePath, string destinationFilePath, byte[] key)
    {
        // Decrypt the source file and write it to the destination file.
        using var sourceStream = File.OpenRead(sourceFilePath);
        using var destinationStream = File.Create(destinationFilePath);
        using var provider = new AesCryptoServiceProvider();
        var iv = new byte[provider.IV.Length];
        var read = sourceStream.Read(iv, 0, iv.Length);
        using (var cryptoTransform = provider.CreateDecryptor(key, iv))
        using (var cryptoStream = new CryptoStream(sourceStream, cryptoTransform, CryptoStreamMode.Read))
        {
            cryptoStream.CopyTo(destinationStream);
        }
    }
}
