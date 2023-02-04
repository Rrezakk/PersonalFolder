namespace PersonalFolder.Interfaces;

public interface IFileNameEncryptor
{
    public string EncryptName(string fileName, byte[] key);
    public string DecryptName(string encryptedFileName, byte[] key);
}
