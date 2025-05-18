using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class RegisterLayerExtension
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NewspaperDbContext>(options =>
        {
            options.UseSqlServer(
                    configuration.GetConnectionString("MasterDatabase"));
        });
    }
}
