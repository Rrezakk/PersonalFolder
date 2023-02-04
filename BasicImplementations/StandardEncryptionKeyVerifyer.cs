using PersonalFolder.Interfaces;

namespace PersonalFolder.BasicImplementations;

public class StandardEncryptionKeyVerifyer:IEncryptionKeyVerifyer
{
    private readonly string _hashFilePath;
    private readonly IKeyHasher _keyHasher;
    public StandardEncryptionKeyVerifyer(string hashFilePath,IKeyHasher hasher, IKeyHasher keyHasher)
    {
        _hashFilePath = hashFilePath;
        _keyHasher = keyHasher;
    }
    public bool VerifyKey(byte[] key)
    {
        var hashForProvidedKey = _keyHasher.GetHash(key);
        var keyhash = File.ReadAllText(_hashFilePath);
        return hashForProvidedKey == keyhash;
    }
    public void WriteVerificationFile(byte[] key)
    {
        var hashForProvidedKey = _keyHasher.GetHash(key);
        if(File.Exists(_hashFilePath))
            File.Delete(_hashFilePath);
        File.WriteAllText(_hashFilePath,hashForProvidedKey);
    }
}
