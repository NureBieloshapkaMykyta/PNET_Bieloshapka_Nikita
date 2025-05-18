using Core.Entities;
using Shared.Helpers;

namespace Business.Interfaces;

public interface INewspaperService
{
    Task<Result<Newspaper>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Newspaper>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<Newspaper>> CreateAsync(Newspaper newspaper, CancellationToken cancellationToken = default);
    Task<Result<Newspaper>> UpdateAsync(Newspaper newspaper, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
} 