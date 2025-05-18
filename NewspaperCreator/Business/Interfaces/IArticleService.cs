using Core.Entities;
using Shared.Helpers;

namespace Business.Interfaces;

public interface IArticleService
{
    Task<Result<Article>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Article>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Article>>> GetByIssueIdAsync(int issueId, CancellationToken cancellationToken = default);
    Task<Result<Article>> CreateAsync(Article article, CancellationToken cancellationToken = default);
    Task<Result<Article>> UpdateAsync(Article article, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
} 