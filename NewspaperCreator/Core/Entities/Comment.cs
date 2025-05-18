namespace Core.Entities;

public class Comment
{
    public long Id { get; set; }
    public required string Content { get; set; }
    public required long UserId { get; set; }
    public virtual User? User { get; set; }
    public required long ArticleId { get; set; }
    public virtual Article? Article { get; set; }
}
