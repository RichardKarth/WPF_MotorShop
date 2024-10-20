
using MainApp.Shared.Enums;
using MainApp.Shared.Interfaces;


namespace MainApp.Shared.Services;

public class FileService : IFileService
{
    private readonly string _filePath;

    public FileService(string filePath)
    {
        _filePath = filePath;
    }

    public ServiceResponse SaveToFile(string content)
    {
        try
        {
            if (!string.IsNullOrEmpty(content))
            {
                using var sw = new StreamWriter(_filePath);
                sw.WriteLine(content);
                return ServiceResponse.Success;
            }
            return ServiceResponse.Failed;
        }
        catch
        {
            return ServiceResponse.Error;
        }

    }
    public string GetFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using var sr = new StreamReader(_filePath);
                var content = sr.ReadToEnd();

                return content;
            }
            else
            {
                string content = string.Empty!;
                using var sw = new StreamWriter(_filePath);
                sw.WriteLine(content);
                return content;
            }
         
        }
        catch
        {
            return null!;
        }
    }
}
