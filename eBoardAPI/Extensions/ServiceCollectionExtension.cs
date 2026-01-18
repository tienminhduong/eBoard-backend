using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.ClassFund;
using eBoardAPI.Models.FundExpense;
using eBoardAPI.Models.FundIncome;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;
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
            services.AddScoped<IFundIncomeRepository, FundIncomeRepository>();
            services.AddScoped<IFundIncomeDetailRepository, FundIncomeDetailRepository>();
            services.AddScoped<IFundExpenseRepository, FundExpenseRepository>();

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
            services.AddScoped<IFundIncomeService, FundIncomeService>();
            services.AddScoped<IFundIncomeDetailService, FundIncomeDetailService>();
            services.AddScoped<IFundExpenseService, FundExpenseService>();
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

                cfg.CreateMap<CreateFundIncomeDto, FundIncome>()
                    .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));
                cfg.CreateMap<FundIncome, FundIncomeDto>();

                cfg.CreateMap<CreateFundIncomeDetailDto, FundIncomeDetail>()
                    .ForMember(dest => dest.ContributedAt, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));
                cfg.CreateMap<FundIncomeDetail, FundIncomeDetailDto>()
                    .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.FundIncome.EndDate));

                cfg.CreateMap<FundExpenseCreateDto, FundExpense>();
                cfg.CreateMap<FundExpense, FundExpenseDto>();

            }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}