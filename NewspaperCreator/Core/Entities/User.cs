namespace Core.Entities;

public class User
{
    public required long Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public virtual ICollection<Newspaper>? Newspapers { get; set; }
    public virtual ICollection<Article>? Articles { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }

}
