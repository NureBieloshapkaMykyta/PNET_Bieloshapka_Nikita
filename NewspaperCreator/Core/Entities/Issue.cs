using Core.Enum;

namespace Core.Entities;

public class Issue
{
    public required long Id { get; set; }
    public required string Title { get; set; }
    public required DateTime PublishDate { get; set; }
    public required IssueStatus Status { get; set; }
    public required long NewspaperId { get; set; }
    public virtual Newspaper? Newspaper { get; set; }
    public virtual ICollection<Article>? Articles { get; set; }
}
