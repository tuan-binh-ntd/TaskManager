using Microsoft.AspNetCore.Http;

namespace TaskManager.Core.DTOs;

public class FileDetails
{
    public IFormFile? FileDetail { get; set; }
}
