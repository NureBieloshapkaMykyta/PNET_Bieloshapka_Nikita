using AutoMapper;
using Business.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;

namespace Business.Services;

public class NewspaperService : INewspaperService
{
    private readonly NewspaperDbContext _dbContext;
    private readonly IMapper _mapper;

    public NewspaperService(NewspaperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<Newspaper>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var newspaper = await _dbContext.Newspapers
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (newspaper == null)
        {
            return new Result<Newspaper>(false, "Newspaper not found");
        }

        return new Result<Newspaper>(true, data: newspaper);
    }

    public async Task<Result<IEnumerable<Newspaper>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var newspapers = await _dbContext.Newspapers
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Newspaper>>(true, data: newspapers);
    }

    public async Task<Result<Newspaper>> CreateAsync(Newspaper newspaper, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newspaper.Title))
        {
            return new Result<Newspaper>(false, "Newspaper name cannot be empty");
        }

        if (await _dbContext.Newspapers.AnyAsync(n => n.Title == newspaper.Title, cancellationToken))
        {
            return new Result<Newspaper>(false, "Newspaper with this name already exists");
        }

        await _dbContext.Newspapers.AddAsync(newspaper, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Newspaper>(true, data: newspaper);
    }

    public async Task<Result<Newspaper>> UpdateAsync(Newspaper newspaper, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newspaper.Title))
        {
            return new Result<Newspaper>(false, "Newspaper name cannot be empty");
        }

        var existingNewspaper = await _dbContext.Newspapers
            .FirstOrDefaultAsync(n => n.Id == newspaper.Id, cancellationToken);

        if (existingNewspaper == null)
        {
            return new Result<Newspaper>(false, "Newspaper not found");
        }

        if (await _dbContext.Newspapers.AnyAsync(n => n.Title == newspaper.Title && n.Id != newspaper.Id, cancellationToken))
        {
            return new Result<Newspaper>(false, "Newspaper with this name already exists");
        }

        _dbContext.Entry(existingNewspaper).CurrentValues.SetValues(newspaper);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Newspaper>(true, data: newspaper);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var newspaper = await _dbContext.Newspapers
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (newspaper == null)
        {
            return new Result<bool>(false, "Newspaper not found");
        }

        _dbContext.Newspapers.Remove(newspaper);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true, data: true);
    }
} 