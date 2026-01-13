using eBoardAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Extensions;


public static class IServiceCollectionExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "eBoard API", Version = "v1" });
        });
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            options.UseNpgsql(connectionString);
        });
        return services;
    }
}