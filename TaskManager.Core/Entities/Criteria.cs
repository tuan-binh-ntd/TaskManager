using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Criteria : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Configuration { get; set; } = string.Empty;

        //Relationship
        public ICollection<FilterCriteria>? FilterCriterias { get; set; }
    }
}
