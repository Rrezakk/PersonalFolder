namespace PersonalFolder.Interfaces;

public interface IKeyHasher
{
    public string GetHash(byte[] key);
}
