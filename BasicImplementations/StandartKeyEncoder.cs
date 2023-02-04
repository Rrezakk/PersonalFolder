using PersonalFolder.Interfaces;
using System.Security.Cryptography;

namespace PersonalFolder.BasicImplementations;

public class StandartKeyEncoder:IKeyEncoder
{
    public byte[] Encode(string keyword)
    {
        byte[] key;
        try
        {
            key = Convert.FromBase64String(keyword);
        }
        catch (System.FormatException)
        {
            var password = keyword;
            var salt = Convert.FromBase64String("GulO8InaX2CwJw ==");
            using var converter = new Rfc2898DeriveBytes(password, salt);
            key = converter.GetBytes(32);
        }
        return key;
    }
}
