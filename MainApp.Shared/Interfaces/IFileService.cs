using MainApp.Shared.Enums;

namespace MainApp.Shared.Interfaces
{
    public interface IFileService
    {
        string GetFromFile();
        ServiceResponse SaveToFile(string content);
    }
}