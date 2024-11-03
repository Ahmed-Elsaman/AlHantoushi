namespace AlHantoushi.Core.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
