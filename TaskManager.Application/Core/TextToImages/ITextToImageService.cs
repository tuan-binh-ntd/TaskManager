namespace TaskManager.Application.Core.TextToImages;

public interface ITextToImageService
{
    Task<string> GenerateImageAsync(string text);
}
