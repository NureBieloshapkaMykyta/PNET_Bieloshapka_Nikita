using AutoMapper;
using Business.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;

namespace Business.Services;

public class ArticleService : IArticleService
{
    private readonly NewspaperDbContext _dbContext;
    private readonly IMapper _mapper;

    public ArticleService(NewspaperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<Article>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _dbContext.Articles
            .Include(a => a.Issue)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (article == null)
        {
            return new Result<Article>(false, "Article not found");
        }

        return new Result<Article>(true, data: article);
    }

    public async Task<Result<IEnumerable<Article>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var articles = await _dbContext.Articles
            .Include(a => a.Issue)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Article>>(true, data: articles);
    }

    public async Task<Result<IEnumerable<Article>>> GetByIssueIdAsync(int issueId, CancellationToken cancellationToken = default)
    {
        var articles = await _dbContext.Articles
            .Include(a => a.Issue)
            .Where(a => a.IssueId == issueId)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Article>>(true, data: articles);
    }

    public async Task<Result<Article>> CreateAsync(Article article, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(article.Title))
        {
            return new Result<Article>(false, "Article title cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(article.Content))
        {
            return new Result<Article>(false, "Article content cannot be empty");
        }

        var issue = await _dbContext.Issues
            .FirstOrDefaultAsync(n => n.Id == article.IssueId, cancellationToken);

        if (issue == null)
        {
            return new Result<Article>(false, "Issue not found");
        }

        await _dbContext.Articles.AddAsync(article, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Article>(true, data: article);
    }

    public async Task<Result<Article>> UpdateAsync(Article article, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(article.Title))
        {
            return new Result<Article>(false, "Article title cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(article.Content))
        {
            return new Result<Article>(false, "Article content cannot be empty");
        }

        var existingArticle = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == article.Id, cancellationToken);

        if (existingArticle == null)
        {
            return new Result<Article>(false, "Article not found");
        }

        var issue = await _dbContext.Issues
            .FirstOrDefaultAsync(n => n.Id == article.IssueId, cancellationToken);

        if (issue == null)
        {
            return new Result<Article>(false, "Issue not found");
        }

        _dbContext.Entry(existingArticle).CurrentValues.SetValues(article);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Article>(true, data: article);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (article == null)
        {
            return new Result<bool>(false, "Article not found");
        }

        _dbContext.Articles.Remove(article);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true, data: true);
    }
} 