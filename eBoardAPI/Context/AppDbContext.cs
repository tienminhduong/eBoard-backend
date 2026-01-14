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
}