namespace TaskManager.Core.Core;

public abstract class BaseEntity : IHasCreationTime, IHasModificationTime, IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual Guid Id { get; set; }
    public virtual DateTime? ModificationTime { get; set; }
    public virtual DateTime CreationTime { get; set; }
}
