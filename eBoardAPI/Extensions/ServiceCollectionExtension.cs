using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Class;
using eBoardAPI.Repositories;
using eBoardAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eBoard API", Version = "v1" });
            });
            return services;
        }

        public IServiceCollection AddDatabase()
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable(EnvKey.DATABASE_CONNECTION_STRING);
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
        
        public IServiceCollection AddAutoMapper()
        {
            var licenseKey = Environment.GetEnvironmentVariable(EnvKey.AUTOMAPPER_LICENSE_KEY);
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = licenseKey;

                cfg.CreateMap<Class, ClassInfoDto>()
                    .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher!.FullName))
                    .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade!.Name));

            }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}