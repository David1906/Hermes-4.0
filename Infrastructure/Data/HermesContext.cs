using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Defects;
using Infrastructure.Data.Features.Errors;
using Infrastructure.Data.Features.Logfiles;
using Infrastructure.Data.Features.Operations;
using Infrastructure.Data.Features.Panels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class HermesContext : DbContext
{
    public DbSet<PanelDto> Panels { get; set; }
    public DbSet<BoardDto> Boards { get; set; }
    public DbSet<LogfileDto> Logfiles { get; set; }
    public DbSet<DefectDto> Defects { get; set; }
    public DbSet<OperationDto> Operations { get; set; }
    public DbSet<ErrorDto> Errors { get; set; }


    public string FilePath { get; init; }
    public string ConnectionString { get; init; }

    public HermesContext()
    {
        FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Hermes4",
            "dbSqliteInfrastructure.db");
        ConnectionString = $"Filename={FilePath}";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString: ConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    public void Migrate()
    {
        if (Database.GetPendingMigrations().Any())
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PanelDto>().ToTable("Panels");
        modelBuilder.Entity<BoardDto>().ToTable("Boards");
        modelBuilder.Entity<LogfileDto>().ToTable("Logfiles");
        modelBuilder.Entity<DefectDto>().ToTable("Defects");
        modelBuilder.Entity<OperationDto>().ToTable("Operations");
        modelBuilder.Entity<ErrorDto>().ToTable("Errors");
    }
}