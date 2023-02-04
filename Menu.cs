using PersonalFolder.BasicImplementations;
using PersonalFolder.Interfaces;
using System.Drawing;
using Console = Colorful.Console;
namespace PersonalFolder;
public class Menu
{
    private readonly IKeyEncoder _keyEncoder = new StandartKeyEncoder();
    private readonly IKeyHasher _keyHasher = new StandardKeyHasher();
    private readonly IEncryptionKeyVerifyer _verifyer;
    private byte[] _key;
    private readonly IFileNameEncryptor _fileNameEncryptor = new StandardFileNameEncryptor();
    private readonly IFileNameLegalizer _fileNameLegalizer = new StandardFileNameLegalizer();
    private readonly IFileEncryptor _fileEncryptor = new StandardFileEncryptor();
    private readonly IFileDeleter _fileDeleter = new SafeFileDeleter();

    #region Menu staff
    private bool _cancelation;
    public delegate void MenuDelegateType();
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
    public Menu(string password)
    {
        _key = _keyEncoder.Encode(password);
        _verifyer = new StandardEncryptionKeyVerifyer(Program.PasswordHashFilePath,_keyHasher);
        _cancelation = false;
    }
    private void ExecuteIfProvided(MenuDelegateType method)
    {
        if (_verifyer.VerifyKey(_key))
        {
            method?.Invoke();
        }
        else
        {
            Console.WriteLine("Password is not valid! Denied",Color.Red);
        }
    }
    #endregion
    
    private void Tick()
    {
        Console.WriteLine("0:Init");
        Console.WriteLine("1:Decrypt");
        Console.WriteLine("2:AddFiles");
        Console.WriteLine("3:Restore");
        Console.WriteLine("4:Re-enter password");
        Console.WriteLine("5:Leave");
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
                    ExecuteIfProvided(DecryptAllFiles);
                    break;
                case 2:
                    ExecuteIfProvided(EncryptAllFiles);
                    break;
                case 3:
                    ExecuteIfProvided(Restore);
                    break;
                case 4:
                    ReEnterPassword();
                    break;
                case 5:
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
    

    #region Implementations
    private void EncryptAllFilesT()
    {
        Console.Write("Enter folder/file path: ");
        var path = Console.ReadLine()??"";
        try
        {
            var attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                var files = Directory.GetFiles(path);
                foreach (var filePath in files)
                {
                    EncryptSingleFile(filePath);
                }
            }
            else
            {
                EncryptSingleFile(path);
            }
            Console.WriteLine("Sucessfully encrypted!");
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Error encrypting!");
        }
        Console.Write($"If you want to delete source files safely, type: ");
        Console.WriteLine("delete",Color.Red);
        Console.WriteLine($"Make sure that your files encrypted successfully before! You will not able to recover old files",Color.Red);
        var choice = Console.ReadLine();
        if (choice.Trim(' ') == "delete")
        {
            //safe delete
            foreach (var file in Directory.GetFiles(path))
            {
                _fileDeleter.DeleteFile(file);
            }
            Console.WriteLine($"Deleted.",Color.Red);
        }
        else
            Console.WriteLine($"Not deleted.",Color.Aquamarine);
    }
    private void EncryptSingleFile(string fileName)
    {
        var filename = Path.GetFileName(fileName);
        var encryptedFileName = _fileNameEncryptor.EncryptName(filename,_key);
        var legalEncryptedFileName = _fileNameLegalizer.Legalize(encryptedFileName);
        var destinationFilePath = Program.LockerDirectoryPath + legalEncryptedFileName;
        _fileEncryptor.Encrypt(fileName,destinationFilePath,_key);
    }
    private void DecryptAllFilesT()
    {
        var files = Directory.GetFiles(Program.LockerDirectoryPath);
        foreach (var filePath in files)
        {
            DecryptSingleFile(filePath);
        }
    }
    private void DecryptSingleFile(string fileName)
    {
        var sourceFileName = Path.GetFileName(fileName);
        var restoredLegalFileName = _fileNameLegalizer.FromLegalized(sourceFileName);
        var decryptedFileName = _fileNameEncryptor.DecryptName(restoredLegalFileName,_key);
        var destinationFilePath = Program.UnlockedFolderPath + decryptedFileName;
        _fileEncryptor.Decrypt(fileName,destinationFilePath,_key);
    }
    #endregion
    
    #region Menu choices
    private void Init()
    {
        _verifyer.WriteVerificationFile(_key);
        EncryptAllFilesT();
    }
    private void EncryptAllFiles()
    {
        EncryptAllFilesT();
    }
    private void DecryptAllFiles()
    {
        DecryptAllFilesT();
    }
    private void ReEnterPassword()
    {
        Console.Write($"Enter your password: ");
        var password = Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine($"Wrong password characters - not affected");
            return;
        }
        _key = _keyEncoder.Encode(password);
    }
    private static void Restore()
    {
        Console.WriteLine("Not available now",Color.Red);
    }
    #endregion
}