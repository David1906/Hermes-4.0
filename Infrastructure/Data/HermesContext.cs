using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Defects;
using Infrastructure.Data.Features.Logfiles;
using Infrastructure.Data.Features.Panels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class HermesContext : DbContext
{
    public DbSet<PanelDbModel> Panels { get; set; }
    public DbSet<BoardDbModel> Boards { get; set; }
    public DbSet<LogfileDbModel> Logfiles { get; set; }
    public DbSet<DefectDbModel> Defects { get; set; }

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
        modelBuilder.Entity<PanelDbModel>().ToTable("Panels");
        modelBuilder.Entity<BoardDbModel>().ToTable("Boards");
        modelBuilder.Entity<LogfileDbModel>().ToTable("Logfiles");
        modelBuilder.Entity<DefectDbModel>().ToTable("Defects");
    }
}