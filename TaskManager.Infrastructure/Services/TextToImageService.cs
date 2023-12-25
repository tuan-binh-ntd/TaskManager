using Microsoft.Extensions.Options;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services;

public class TextToImageService : ITextToImageService
{
    private readonly TextToImageAISettings _textToImageAISettings;

    public TextToImageService(
        IOptionsMonitor<TextToImageAISettings> optionsMonitor
        )
    {
        _textToImageAISettings = optionsMonitor.CurrentValue;
    }

    public async Task<string> GenerateImageAsync(string text)
    {
        using HttpClient client = new();
        using MultipartFormDataContent form = new()
        {
            { new StringContent("shot of vaporwave fashion dog in miami"), "prompt" }
        };

        client.DefaultRequestHeaders.Add("x-api-key", _textToImageAISettings.APIKey);

        using HttpResponseMessage response = await client.PostAsync(_textToImageAISettings.APIUrl, form);
        if (response.IsSuccessStatusCode)
        {
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
            // 'imageBytes' is a binary representation of the returned image
            Console.WriteLine("Image received. Do something with it.");
        }
        else
        {
            Console.WriteLine($"HTTP error! Status: {response.StatusCode}");
            // Handle the error appropriately
        }
        return string.Empty;
    }
}
