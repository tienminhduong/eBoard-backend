using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.ClassFund;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Schedule;
using eBoardAPI.Models.Student;
using eBoardAPI.Models.Subject;
using eBoardAPI.Models.Teacher;
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
            services.AddScoped<IClassFundRepository, ClassFundRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IClassFundService, ClassFundService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            return services;
        }
        
        public IServiceCollection AddAutoMapper()
        {
            var licenseKey = Environment.GetEnvironmentVariable(EnvKey.AUTOMAPPER_LICENSE_KEY);
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = licenseKey;
                
                cfg.CreateMap<Parent, ParentInfoDto>();

                cfg.CreateMap<Student, StudentInfoDto>()
                    .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => StringHelper.ParseFullAddress(src)));
                cfg.CreateMap<CreateStudentDto, Student>();

                cfg.CreateMap<Class, ClassInfoDto>()
                    .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.FullName))
                    .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Name));
                cfg.CreateMap<CreateClassDto, Class>();

                cfg.CreateMap<UpdateTeacherInfoDto, Teacher>();

                cfg.CreateMap<Teacher, TeacherInfoDto>();
                cfg.CreateMap<ClassFund, ClassFundDto>()
                    .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                    .ForMember(dest => dest.AcademicYear,
                        opt => opt.MapFrom(src => StringHelper.ParseAcademicYear(src.Class)));

                cfg.CreateMap<CreateClassPeriodDto, ClassPeriod>()
                    .ForSourceMember(src => src.Subject, opt => opt.DoNotValidate())
                    .ForMember(dest => dest.Subject, opt => opt.Ignore());
                
                cfg.CreateMap<UpdateClassPeriodDto, ClassPeriod>()
                    .ForSourceMember(src => src.Subject, opt => opt.DoNotValidate())
                    .ForMember(dest => dest.Subject, opt => opt.Ignore());
                
                cfg.CreateMap<ClassPeriod, ClassPeriodDto>();
                cfg.CreateMap<Subject, SubjectDto>();

            }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}