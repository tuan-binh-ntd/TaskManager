using TaskManager.Core.Helper;

namespace TaskManager.Core.DTOs
{
    public class GetProjectByFilterDto : PaginationInput
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
