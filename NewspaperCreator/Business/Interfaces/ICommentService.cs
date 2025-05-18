using Core.Entities;
using Shared.Helpers;

namespace Business.Interfaces;

public interface ICommentService
{
    Task<Result<Comment>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Comment>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Comment>>> GetByArticleIdAsync(int articleId, CancellationToken cancellationToken = default);
    Task<Result<Comment>> CreateAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<Result<Comment>> UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
} 