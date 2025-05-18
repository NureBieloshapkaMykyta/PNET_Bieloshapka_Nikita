using System.Collections;

namespace Core.Entities;

public class Newspaper
{
    public required long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required long OwnerId { get; set; }
    public virtual User? Owner { get; set; }
    public virtual ICollection<Issue>? Issues { get; set; }
}
