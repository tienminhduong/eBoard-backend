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
}