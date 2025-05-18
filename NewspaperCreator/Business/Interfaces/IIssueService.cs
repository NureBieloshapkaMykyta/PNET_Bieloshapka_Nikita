using Core.Entities;
using Shared.Helpers;

namespace Business.Interfaces;

public interface IIssueService
{
    Task<Result<Issue>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Issue>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Issue>>> GetByNewspaperIdAsync(int newspaperId, CancellationToken cancellationToken = default);
    Task<Result<Issue>> CreateAsync(Issue issue, CancellationToken cancellationToken = default);
    Task<Result<Issue>> UpdateAsync(Issue issue, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
} 