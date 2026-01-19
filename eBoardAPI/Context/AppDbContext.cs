using eBoardAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<InClass> InClasses { get; set; }
    public DbSet<ClassFund> ClassFunds { get; set; }
    public DbSet<FundIncome> FundIncomes { get; set; }
    public DbSet<FundIncomeDetail> FundIncomeDetails { get; set; }
    public DbSet<FundExpense> FundExpenses { get; set; }
    public DbSet<ClassPeriod> ClassPeriods { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ScheduleSetting> ScheduleSettings { get; set; }
    public DbSet<ScheduleSettingDetail> ScheduleSettingDetails { get; set; }
    public DbSet<ScoreSheet> ScoreSheets { get; set; }
    public DbSet<ScoreSheetDetail> ScoreSheetDetails { get; set; }
    public DbSet<Violation> Violations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClassPeriod>()
            .HasIndex(e => new { e.IsMorningPeriod, e.PeriodNumber, e.DayOfWeek, e.ClassId }).IsUnique();
    }
}