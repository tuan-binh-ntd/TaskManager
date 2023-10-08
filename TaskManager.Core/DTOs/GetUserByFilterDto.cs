using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class GetUserByFilterDto
    {
        public string? Name { get; set; } = string.Empty;
   
    }
}
