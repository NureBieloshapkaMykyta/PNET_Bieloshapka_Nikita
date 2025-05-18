namespace Core.Entities;

public class Article
{
    public required long Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required long IssueId { get; set; }
    public virtual Issue? Issue { get; set; }
    public required long? AuthorId { get; set; }
    public virtual User? Author { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Comment>? Comments { get; set; }
}
