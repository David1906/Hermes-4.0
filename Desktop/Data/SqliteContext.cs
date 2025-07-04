using Desktop.Features.Boards.Infrastructure;
using Desktop.Features.Defects.Infrastructure;
using Desktop.Features.Logfiles.Infrastructure;
using Desktop.Features.OperationTasks.Infrastructure;
using Desktop.Features.Operations.Infrastructure;
using Desktop.Features.Panels.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System;
using Desktop.Features.Users.Infrastructure;

namespace Desktop.Data;

public class SqliteContext : DbContext
{
    public DbSet<OperationDbModel> Operations { get; set; }
    public DbSet<OperationTaskDbModel> OperationTasks { get; set; }
    public DbSet<PanelDbModel> Panels { get; set; }
    public DbSet<BoardDbModel> Boards { get; set; }
    public DbSet<LogfileDbModel> Logfiles { get; set; }
    public DbSet<DefectDbModel> Defects { get; set; }
    public DbSet<UserDbModel> Users { get; set; }

    public string FilePath { get; init; }
    public string ConnectionString { get; init; }

    public SqliteContext()
    {
        FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Hermes4",
            "dbSqlite.db");
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
        modelBuilder.Entity<OperationDbModel>().ToTable("Operations");
        modelBuilder.Entity<OperationTaskDbModel>().ToTable("OperationTasks");
        modelBuilder.Entity<PanelDbModel>().ToTable("Panels");
        modelBuilder.Entity<BoardDbModel>().ToTable("Boards");
        modelBuilder.Entity<LogfileDbModel>().ToTable("Logfiles");
        modelBuilder.Entity<DefectDbModel>().ToTable("Defects");
        modelBuilder.Entity<UserDbModel>().ToTable("Users");
    }
}