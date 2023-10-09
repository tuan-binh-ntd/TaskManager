using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.ViewModel
{
    public class IssueTypeViewModel : BaseDto<IssueType, IssueTypeViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public byte Level { get; set; }
    }
}
