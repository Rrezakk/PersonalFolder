using PersonalFolder.Interfaces;

namespace PersonalFolder.BasicImplementations;

public class StandardFileNameLegalizer:IFileNameLegalizer
{
    private static readonly Dictionary<string, string> map = new()
    {
        {"<", "($lessthan$)"},
        {">", "($greater than$)"},
        {":", "($colon$)"},
        {"\"", "($double quote$)"},
        {"/", "($forward slash$)"},
        {"\\", "($back slash$)"},
        {"|", "($vertical bar or pipe$)"},
        {"?", "($question mark$)"},
        {"*", "($asterisk$)"}
    };
    public string Legalize(string fileName)
    {
        return map.Aggregate(fileName, (current, pair) => current.Replace(pair.Key, pair.Value));
    }
    public string FromLegalized(string legalFileName)
    {
        return map.Aggregate(legalFileName, (current, pair) => current.Replace(pair.Value, pair.Key));
    }
}
