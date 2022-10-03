using Console = Colorful.Console;
namespace PersonalFolder;
internal static class Program
{
    public static readonly string PasswordHashFilePath = Environment.CurrentDirectory + "\\hash.lsf";
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