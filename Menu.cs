using System.Drawing;
using Console = Colorful.Console;
namespace PersonalFolder;
public class Menu
{
    private bool _cancelation;
    private static EncryptionHelper _enc = null!;
    public delegate void MenuDelegateType();
    public Menu(string password)
    {
        _cancelation = false;
        _enc = new EncryptionHelper(password);
    }
    private static void ErrorMessage()
    {
        Console.WriteLine("Invalid choice! Try again",Color.Red);
    }
    public void Show()
    {
        while (!_cancelation)
        {
            Tick();
        }
    }
    private void Tick()
    {
        Console.WriteLine("0:Init");
        Console.WriteLine("1:Decrypt");
        Console.WriteLine("2:AddFiles");
        Console.WriteLine("3:Restore");
        Console.WriteLine("4:Leave");
        Console.Write("Choose action: ");
        var choice = Console.ReadLine()??"";
        if (int.TryParse(choice, out var choiceNum))
        {
            switch (choiceNum)
            {
                case 0:
                    Init();
                    break;
                case 1:
                    ExecuteIfProvided(DecryptFiles);
                    break;
                case 2:
                    ExecuteIfProvided(AddFiles);
                    break;
                case 3:
                    ExecuteIfProvided(Restore);
                    break;
                case 4:
                    _cancelation = true;
                    break;
                default:
                    ErrorMessage();
                    break;
            }
        }
        else
            ErrorMessage();
        
    }
    private static void ExecuteIfProvided(MenuDelegateType method)
    {
        if (CheckPass(_enc.Key4))
        {
            method?.Invoke();
        }
        else
        {
            Console.WriteLine("Password is not valid! Denied",Color.Red);
        }
    }
    private static bool CheckPass(string password)
    {
        try
        {
            return HashingHelper.ReadHashFile(Program.PasswordHashFilePath) == HashingHelper.Sha256Encrypt(password);//sha256 hash checking 
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        
    }
    private static void AddFilesT()
    {
        Console.Write("Enter folder/file path: ");
        var path = Console.ReadLine()??"";
        try
        {
            var attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                var filess = Directory.GetFiles(path);
                foreach (var filePath in filess)
                {
                    _enc.EncryptFile(filePath);
                }
            }
            else
            {
                _enc.EncryptFile(path);
            }
            Console.WriteLine("Sucessfully encrypted!");
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Error encrypting!");
        }
    }
    private static void Init()
    {
        HashingHelper.Sha256EncryptToFile(_enc.Key4, Program.PasswordHashFilePath);
        AddFilesT();
    }
    private static void DecryptFiles()
    {
        if (!CheckPass(_enc.Key4))
            return;
        var files = Directory.GetFiles(Program.LockerDirectoryPath);
        foreach (var filePath in files)
        {
            _enc.DecryptFile(filePath);
        }
    }
    private static void AddFiles()
    {
        if (!CheckPass(_enc.Key4))
            return;
        AddFilesT();
    }
    private static void Restore()
    {
        if (!CheckPass(_enc.Key4))
            return;
        Console.WriteLine("Not available now",Color.Red);
    }
}