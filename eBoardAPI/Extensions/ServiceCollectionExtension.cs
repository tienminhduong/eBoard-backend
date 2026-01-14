using eBoardAPI.Context;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Repositories;
using eBoardAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Extensions;


public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSwagger()
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "eBoard API", Version = "v1" });
            });
            return services;
        }

        public IServiceCollection AddDatabase()
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
                options.UseNpgsql(connectionString);
            });
            return services;
        }
        
        public IServiceCollection AddRepositories()
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IClassService, ClassService>();
            return services;
        }
    }
}