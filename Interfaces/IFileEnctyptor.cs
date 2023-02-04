namespace PersonalFolder.Interfaces;

public interface IFileEncryptor
{
    public void Encrypt(string sourceFilePath, string destinationFileName, byte[]? key);
    public void Decrypt(string sourceFilePath, string destinationFileName, byte[] key);
}
