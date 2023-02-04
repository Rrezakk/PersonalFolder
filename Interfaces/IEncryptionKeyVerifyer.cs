namespace PersonalFolder.Interfaces;

public interface IEncryptionKeyVerifyer
{
    public bool VerifyKey(byte[] key);
    public void WriteVerificationFile(byte[] key);
}
