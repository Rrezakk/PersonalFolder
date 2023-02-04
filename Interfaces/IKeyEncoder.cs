namespace PersonalFolder.Interfaces;

public interface IKeyEncoder
{
    public byte[] Encode(string keyword);
}
