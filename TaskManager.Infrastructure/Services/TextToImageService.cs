using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services;

public class TextToImageService : ITextToImageService
{
    public TextToImageService(

        )
    {
    }

    public async Task<string> GenerateImageAsync(string text)
    {
        string apiKey = "YOUR_API_KEY"; // Replace with your actual API key
        string apiUrl = "https://clipdrop-api.co/text-to-image/v1";

        using HttpClient client = new();
        using MultipartFormDataContent form = new()
        {
            { new StringContent("shot of vaporwave fashion dog in miami"), "prompt" }
        };

        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

        using HttpResponseMessage response = await client.PostAsync(apiUrl, form);
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
    }
}
