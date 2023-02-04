namespace PersonalFolder.Interfaces;

public interface IFileEncryptor
{
    public void Encrypt(string sourceFilePath, string destinationFilePath, byte[]? key);
    public void Decrypt(string sourceFilePath, string destinationFilePath, byte[] key);
}
