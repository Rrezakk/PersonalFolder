namespace PersonalFolder.Interfaces;

public interface IFileNameLegalizer
{
    public string Legalize(string fileName);
    public string FromLegalized(string legalFileName);
}
