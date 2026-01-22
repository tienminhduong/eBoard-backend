using AutoMapper;
using eBoardAPI.Consts;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models;
using eBoardAPI.Models.AbsentRequest;
using eBoardAPI.Models.Activity;
using eBoardAPI.Models.Attendance;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.ClassFund;
using eBoardAPI.Models.ExamSchedule;
using eBoardAPI.Models.FundExpense;
using eBoardAPI.Models.FundIncome;
using eBoardAPI.Models.Notification;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Schedule;
using eBoardAPI.Models.ScoreSheet;
using eBoardAPI.Models.Student;
using eBoardAPI.Models.Subject;
using eBoardAPI.Models.Teacher;
using eBoardAPI.Models.Violation;
using eBoardAPI.Repositories;
using eBoardAPI.Services;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Refit;

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
                options.UseExceptionProcessor();
            });
            return services;
        }
        
        public IServiceCollection AddRepositories()
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IClassFundRepository, ClassFundRepository>();
            services.AddScoped<IFundIncomeRepository, FundIncomeRepository>();
            services.AddScoped<IFundIncomeDetailRepository, FundIncomeDetailRepository>();
            services.AddScoped<IFundExpenseRepository, FundExpenseRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<IViolationRepository, ViolationRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAbsentRequestRepository, AbsentRequestRepository>();
            services.AddScoped<IExamScheduleRepository, ExamScheduleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IParentNotificationRepository, ParentNotificationRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IClassFundService, ClassFundService>();
            services.AddScoped<IFundIncomeService, FundIncomeService>();
            services.AddScoped<IFundIncomeDetailService, FundIncomeDetailService>();
            services.AddScoped<IFundExpenseService, FundExpenseService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IViolationService, ViolationService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IExamScheduleService, ExamScheduleService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IParentNotificationService, ParentNotificationService>();
            return services;
        }
        
        public IServiceCollection AddAutoMapper()
        {
            var licenseKey = Environment.GetEnvironmentVariable(EnvKey.AUTOMAPPER_LICENSE_KEY);
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = licenseKey;
                
                cfg.CreateMap<Subject, SubjectDto>();
                cfg.CreateMap<Parent, ParentInfoDto>();
                cfg.CreateMap<UpdateTeacherInfoDto, Teacher>();
                cfg.CreateMap<Teacher, TeacherInfoDto>();
                
                cfg.CreateMap<ScheduleSetting, ScheduleSettingDto>();
                cfg.CreateMap<ScheduleSettingDetail, ScheduleSettingDetailDto>();
                cfg.CreateMap<ParentNotification, ParentNotificationDto>();
                
                AddStudentDtoMappings(cfg);
                AddClassDtoMappings(cfg);
                AddFundDtoMappings(cfg);
                AddClassPeriodDtoMappings(cfg);
                AddScoreSheetDtoMappings(cfg);
                AddAttendanceDtoMappings(cfg);
                AddActivityDtoMapping(cfg);
                
                AddViolationDtoMapping(cfg);
                AddExamScheduleDtoMappings(cfg);
            }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }


        public IServiceCollection AddCorsPolicy()
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalHost", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return services;
        }

        public IServiceCollection AddProvinceApiClient()
        {
            var baseUrl = Environment.GetEnvironmentVariable(EnvKey.VIETNAM_PROVINCE_API_URL);
            services.AddRefitClient<IAddressService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(baseUrl ?? "");
                });
            return services;
        }
    }

    private static void AddStudentDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Student, StudentInfoDto>()
            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => StringHelper.ParseFullAddress(src)));
        cfg.CreateMap<CreateStudentDto, Student>();
    }
    
    private static void AddClassDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Class, ClassInfoDto>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.FullName))
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Name));
        cfg.CreateMap<CreateClassDto, Class>();
    }

    private static void AddFundDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ClassFund, ClassFundDto>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
            .ForMember(dest => dest.AcademicYear,
                opt => opt.MapFrom(src => StringHelper.ParseAcademicYear(src.Class)));

        cfg.CreateMap<CreateFundIncomeDto, FundIncome>()
            .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));
        cfg.CreateMap<FundIncome, FundIncomeDto>();
        cfg.CreateMap<UpdateFundIncomeDto, FundIncome>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom((src, dest) => src.StartDate == DateOnly.MinValue ? dest.StartDate : src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom((src, dest) => src.EndDate == DateOnly.MinValue ? dest.EndDate : src.EndDate))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        cfg.CreateMap<CreateFundIncomeDetailDto, FundIncomeDetail>()
            .ForMember(dest => dest.ContributedAt, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));
        cfg.CreateMap<FundIncomeDetail, FundIncomeDetailDto>()
            .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.FundIncome.EndDate));
        cfg.CreateMap<ContributeFundIncomeDto, FundIncomeDetail>()
            .ForMember(dest => dest.ContributedAt, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));

        cfg.CreateMap<FundExpenseCreateDto, FundExpense>();
        cfg.CreateMap<FundExpense, FundExpenseDto>();
    }

    private static void AddClassPeriodDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<CreateClassPeriodDto, ClassPeriod>()
            .ForSourceMember(src => src.Subject, opt => opt.DoNotValidate())
            .ForMember(dest => dest.Subject, opt => opt.Ignore());
                
        cfg.CreateMap<UpdateClassPeriodDto, ClassPeriod>()
            .ForSourceMember(src => src.Subject, opt => opt.DoNotValidate())
            .ForMember(dest => dest.Subject, opt => opt.Ignore());
                
        cfg.CreateMap<ClassPeriod, ClassPeriodDto>();
    }
    
    private static void AddScoreSheetDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ScoreSheet, StudentScoreSummaryDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => $"{src.Student.LastName} {src.Student.FirstName}"))
            .ForMember(dest => dest.FinalGrade,
                opt => opt.MapFrom(src => StringHelper.ConductAndScoreGradeToFinalGrade(src.Conduct, src.Grade)));

        cfg.CreateMap<ScoreSheet, StudentScoreSheetDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => $"{src.Student.LastName} {src.Student.FirstName}"))
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
            .ForMember(dest => dest.AcademicYear,
                opt => opt.MapFrom(src => $"{src.Class.StartDate.Year}-{src.Class.EndDate.Year}"))
            .ForMember(dest => dest.SubjectScores, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.RankInClass,
                opt => opt.MapFrom(src => $"{src.Rank}/{src.Class.CurrentStudentCount}"));

        cfg.CreateMap<ScoreSheetDetail, SubjectScoreDto>()
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
    }
    
    private static void AddAttendanceDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => src.Student.LastName + " " + src.Student.FirstName));

        cfg.CreateMap<CreateAbsentRequestDto, AbsentRequest>();
        cfg.CreateMap<AbsentRequest, AbsentRequestDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => src.Student.LastName + " " + src.Student.FirstName))
            .ForMember(dest => dest.ClassName,
                opt => opt.MapFrom(src => src.Class.Name));
    }
  
    private static void AddViolationDtoMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Violation, ViolationDto>();
    }

    private static void AddExamScheduleDtoMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<CreateExamScheduleDto, ExamSchedule>();
        cfg.CreateMap<ExamSchedule, ExamScheduleDto>();
        cfg.CreateMap<UpdateExamScheduleDto, ExamSchedule>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
    
    private static void AddActivityDtoMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ExtracurricularActivity, ExtracurricularActivityDto>();
        cfg.CreateMap<ExtracurricularActivity, ParentViewActivityDto>()
            .ForMember(dest => dest.AssignStatus, opt => opt.Ignore());
        cfg.CreateMap<CreateActivityDto, ExtracurricularActivity>();
        cfg.CreateMap<UpdateActivityDto, ExtracurricularActivity>();
        
        cfg.CreateMap<ActivityParticipant, ActivityParticipantDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => $"{src.Student.LastName} {src.Student.FirstName}"));
        
        cfg.CreateMap<AddActivityParticipantDto, ActivityParticipant>();
        cfg.CreateMap<UpdateActivityParticipantDto, ActivityParticipant>();
        
        cfg.CreateMap<ActivitySignIn, ActivitySignInDto>()
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src => $"{src.Student.LastName} {src.Student.FirstName}"));
        cfg.CreateMap<AddActivitySignInDto, ActivitySignIn>()
            .ForMember(dest => dest.SignInTime,
                opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}