using PersonalFolder.Interfaces;

namespace PersonalFolder.BasicImplementations;

public class SafeFileDeleter:IFileDeleter
{
    public bool DeleteFile(string path)
    {
        return SecureDelete(path);
    }
    public static bool SecureDelete(string fullyQualifiedFileName)
    {
        try
        {
            using var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "sdelete";
            p.StartInfo.Arguments = $"/accepteula -q \"{fullyQualifiedFileName}\"";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.UseShellExecute = false;
            p.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Deleting exception: {e}");
            return false;
        }
        return true;
    }
}
