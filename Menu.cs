namespace PersonalFolder;

public class Menu
{
    private bool _cancelation;
    private static EncryptionHelper _enc = null!;
    public Menu(string password)
    {
        _cancelation = false;
        _enc = new EncryptionHelper(password);
    }
    private static void ErrorMessage()
    {
        Console.WriteLine("Invalid choice! Try again");
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
                    DecryptFiles();
                    break;
                case 2:
                    AddFiles();
                    break;
                case 3:
                    Restore();
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

    private static bool CheckPass(string password)
    {
        return false;//sha256 hashing 
    }
    // private static bool CheckPass(string password)
    // {
    //     if (!File.Exists(Program.LockerTestFilePath)) return false;
    //     var localSecret = File.ReadAllText(Program.LockerTestFilePath);
    //     var iss = EncryptionHelper.DecryptText(localSecret, password) == Program.Secret;
    //     Console.WriteLine(iss ? "Password is right" : "Wrong password!");
    //     return iss;
    // }

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
    // private static void GenerateValidationFile()
    // {
    //     File.WriteAllText(Program.LockerTestFilePath, EncryptionHelper.EncryptText(Program.Secret, _enc.Key4));
    // }
    private static void Init()
    {
        //GenerateValidationFile();
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
        Console.WriteLine("Not available now");
    }
}