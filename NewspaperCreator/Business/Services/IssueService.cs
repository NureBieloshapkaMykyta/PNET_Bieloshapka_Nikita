using AutoMapper;
using Business.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;

namespace Business.Services;

public class IssueService : IIssueService
{
    private readonly NewspaperDbContext _dbContext;
    private readonly IMapper _mapper;

    public IssueService(NewspaperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<Issue>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var issue = await _dbContext.Issues
            .Include(i => i.Newspaper)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (issue == null)
        {
            return new Result<Issue>(false, "Issue not found");
        }

        return new Result<Issue>(true, data: issue);
    }

    public async Task<Result<IEnumerable<Issue>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var issues = await _dbContext.Issues
            .Include(i => i.Newspaper)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Issue>>(true, data: issues);
    }

    public async Task<Result<IEnumerable<Issue>>> GetByNewspaperIdAsync(int newspaperId, CancellationToken cancellationToken = default)
    {
        var issues = await _dbContext.Issues
            .Include(i => i.Newspaper)
            .Where(i => i.NewspaperId == newspaperId)
            .ToListAsync(cancellationToken);

        return new Result<IEnumerable<Issue>>(true, data: issues);
    }

    public async Task<Result<Issue>> CreateAsync(Issue issue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(issue.Title))
        {
            return new Result<Issue>(false, "Issue title cannot be empty");
        }

        if (issue.PublishDate == default)
        {
            return new Result<Issue>(false, "Publication date must be specified");
        }

        var newspaper = await _dbContext.Newspapers
            .FirstOrDefaultAsync(n => n.Id == issue.NewspaperId, cancellationToken);

        if (newspaper == null)
        {
            return new Result<Issue>(false, "Newspaper not found");
        }

        if (await _dbContext.Issues.AnyAsync(i => i.Title == issue.Title && i.NewspaperId == issue.NewspaperId, cancellationToken))
        {
            return new Result<Issue>(false, "Issue with this title already exists for this newspaper");
        }

        await _dbContext.Issues.AddAsync(issue, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Issue>(true, data: issue);
    }

    public async Task<Result<Issue>> UpdateAsync(Issue issue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(issue.Title))
        {
            return new Result<Issue>(false, "Issue title cannot be empty");
        }

        if (issue.PublishDate == default)
        {
            return new Result<Issue>(false, "Publication date must be specified");
        }

        var existingIssue = await _dbContext.Issues
            .FirstOrDefaultAsync(i => i.Id == issue.Id, cancellationToken);

        if (existingIssue == null)
        {
            return new Result<Issue>(false, "Issue not found");
        }

        var newspaper = await _dbContext.Newspapers
            .FirstOrDefaultAsync(n => n.Id == issue.NewspaperId, cancellationToken);

        if (newspaper == null)
        {
            return new Result<Issue>(false, "Newspaper not found");
        }

        if (await _dbContext.Issues.AnyAsync(i => i.Title == issue.Title && i.NewspaperId == issue.NewspaperId && i.Id != issue.Id, cancellationToken))
        {
            return new Result<Issue>(false, "Issue with this title already exists for this newspaper");
        }

        _dbContext.Entry(existingIssue).CurrentValues.SetValues(issue);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Issue>(true, data: issue);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var issue = await _dbContext.Issues
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (issue == null)
        {
            return new Result<bool>(false, "Issue not found");
        }

        _dbContext.Issues.Remove(issue);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true, data: true);
    }
} 