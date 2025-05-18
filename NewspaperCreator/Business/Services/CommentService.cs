using AutoMapper;
using Business.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;

namespace Business.Services;

public class CommentService : ICommentService
{
    private readonly NewspaperDbContext _dbContext;
    private readonly IMapper _mapper;

    public CommentService(NewspaperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<Comment>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _dbContext.Comments
            .Include(c => c.Article)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (comment == null)
        {
            return new Result<Comment>(false, "Comment not found");
        }

        return new Result<Comment>(true, data: comment);
    }

    public async Task<Result<IEnumerable<Comment>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var comments = await _dbContext.Comments
            .Include(c => c.Article)
            .Include(c => c.User)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Comment>>(true, data: comments);
    }

    public async Task<Result<IEnumerable<Comment>>> GetByArticleIdAsync(int articleId, CancellationToken cancellationToken = default)
    {
        var comments = await _dbContext.Comments
            .Include(c => c.Article)
            .Include(c => c.User)
            .Where(c => c.ArticleId == articleId)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Comment>>(true, data: comments);
    }

    public async Task<Result<Comment>> CreateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
        {
            return new Result<Comment>(false, "Comment content cannot be empty");
        }

        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == comment.ArticleId, cancellationToken);

        if (article == null)
        {
            return new Result<Comment>(false, "Article not found");
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == comment.UserId, cancellationToken);

        if (user == null)
        {
            return new Result<Comment>(false, "User not found");
        }

        await _dbContext.Comments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Comment>(true, data: comment);
    }

    public async Task<Result<Comment>> UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
        {
            return new Result<Comment>(false, "Comment content cannot be empty");
        }

        var existingComment = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == comment.Id, cancellationToken);

        if (existingComment == null)
        {
            return new Result<Comment>(false, "Comment not found");
        }

        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == comment.ArticleId, cancellationToken);

        if (article == null)
        {
            return new Result<Comment>(false, "Article not found");
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == comment.UserId, cancellationToken);

        if (user == null)
        {
            return new Result<Comment>(false, "User not found");
        }

        _dbContext.Entry(existingComment).CurrentValues.SetValues(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Comment>(true, data: comment);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (comment == null)
        {
            return new Result<bool>(false, "Comment not found");
        }

        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true, data: true);
    }
} 