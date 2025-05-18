using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class RegisterLayerExtension
{
    public static void AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<INewspaperService, NewspaperService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IIssueService, IssueService>();
    }
}
