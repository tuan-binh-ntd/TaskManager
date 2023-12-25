namespace TaskManager.Core.Interfaces.Services;

public interface ITextToImageService
{
    Task<string> GenerateImageAsync(string text);
}
