namespace TaskManager.Infrastructure.Services;

public class TextToImageService : ITextToImageService
{
    private readonly TextToImageAISettings _textToImageAISettings;
    private readonly FileShareSettings _fileShareSettings;

    public TextToImageService(
        IOptionsMonitor<TextToImageAISettings> optionsMonitor,
        IOptionsMonitor<FileShareSettings> optionsMonitor1
        )
    {
        _textToImageAISettings = optionsMonitor.CurrentValue;
        _fileShareSettings = optionsMonitor1.CurrentValue;
    }

    public async Task<string> GenerateImageAsync(string text)
    {
        using HttpClient client = new();
        using MultipartFormDataContent form = new()
        {
            { new StringContent(text), "prompt" }
        };

        client.DefaultRequestHeaders.Add("x-api-key", _textToImageAISettings.APIKey);

        using HttpResponseMessage response = await client.PostAsync(_textToImageAISettings.APIUrl, form);

        if (response.IsSuccessStatusCode)
        {
            byte[] byteArray = await response.Content.ReadAsByteArrayAsync();
            // 'imageBytes' is a binary representation of the returned image

            // Get the configurations and create share object
            ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

            // Create the share if it doesn't already exist
            await share.CreateIfNotExistsAsync();

            // Get a reference to the sample directory
            ShareDirectoryClient directory = share.GetDirectoryClient("AvatarOfUser");

            // Create the directory if it doesn't already exist
            await directory.CreateIfNotExistsAsync();

            // Get a reference to a file and upload it
            ShareFileClient shareFile = directory.GetFileClient($"{Guid.NewGuid()}.png");

            using MemoryStream inputStream = new(byteArray);

            using MemoryStream outputStream = new();

            using Image image = Image.Load(inputStream);

            image.Save(outputStream, new PngEncoder());

            var imageBytes = outputStream.ToArray();

            using Stream imageStream = new MemoryStream(imageBytes);

            shareFile.Create(imageStream.Length);

            int blockSize = CoreConstants.BlockSize;
            long offset = 0;//Define http range offset
            BinaryReader reader = new(imageStream);
            while (true)
            {
                byte[] buffer = reader.ReadBytes(blockSize);
                if (offset == imageStream.Length)
                {
                    break;
                }
                else
                {
                    MemoryStream uploadChunk = new();
                    uploadChunk.Write(buffer, 0, buffer.Length);
                    uploadChunk.Position = 0;

                    HttpRange httpRange = new(offset, buffer.Length);
                    var response1 = shareFile.UploadRange(httpRange, uploadChunk);
                    offset += buffer.Length;//Shift the offset by number of bytes already written
                }
            }
            reader.Close();

            return shareFile.Uri.AbsoluteUri;
        }
        else
        {
            Console.WriteLine($"HTTP error! Status: {response.StatusCode}");
            return string.Empty;
        }
    }
}
