using PersonalFolder.BasicImplementations;
using PersonalFolder.Interfaces;
using Console = Colorful.Console;
namespace PersonalFolder;
internal static class Program
{
    //maybe hashtable for filenames in encrypted file?)))
    //possible encryption problems
    
    
    //It is not safe to use similar keys for encryption files and its names but who cares? It's just security for common users)
    public static readonly string PasswordHashFilePath = Environment.CurrentDirectory + "\\hash.lsf";
    public static readonly string LockerDirectoryPath = Environment.CurrentDirectory + "\\data\\";
    public static readonly string UnlockedFolderPath  = Environment.CurrentDirectory +"\\UnlockedSecretFolder\\";
    private static void CheckDirs()
    {
        if (!Directory.Exists(LockerDirectoryPath))
            Directory.CreateDirectory(LockerDirectoryPath);
        if (!Directory.Exists(UnlockedFolderPath))
            Directory.CreateDirectory(UnlockedFolderPath);
    }
    public static void Main()
    {
        CheckDirs();
        Console.Write($"Enter your password: ");
        var password = Console.ReadLine();
        if (string.IsNullOrEmpty(password)) return;
        var menu = new Menu(password);
        menu.Show();
    }
}