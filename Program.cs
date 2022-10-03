using System.Security.Cryptography;
using System.Text;

namespace PersonalFolder;

internal class Program
{
    public const string Secret = "fawanognoOIH#M*FN*F#WFO(b3w8)";
    //public static readonly string LockerTestFilePath = Environment.CurrentDirectory + "\\lockerFile.lsf";
    public static readonly string LockerDirectoryPath = Environment.CurrentDirectory + "\\data\\";
    public static readonly string DesktopUnlockedFolderPath  = Environment.CurrentDirectory +"\\UnlockedSecretFolder\\";
    private static void CheckDirs()
    {
        if (!Directory.Exists(LockerDirectoryPath))
            Directory.CreateDirectory(LockerDirectoryPath);
        if (!Directory.Exists(DesktopUnlockedFolderPath))
            Directory.CreateDirectory(DesktopUnlockedFolderPath);
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