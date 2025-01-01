using Microsoft.EntityFrameworkCore;
using Quiz.Master.Core.Models;

namespace Quiz.Master.Persistance;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Core.Models.Game> Games { get; set; } = null!;
    public DbSet<MiniGameDefinition> MiniGameDefinitions { get; set; } = null!;
    public DbSet<MiniGameInstance> MiniGameInstances { get; set; } = null!;
    public DbSet<MiniGameInstanceScore> MiniGameInstanceScores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Players");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DeviceId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Core.Models.Game>(entity =>
        {
            entity.ToTable("Games");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasMany(e => e.Players)
                .WithOne(q => q.Game)
                .HasForeignKey(q => q.GameId);
            entity.HasMany(e => e.MiniGames)
                .WithOne(q => q.Game)
                .HasForeignKey(q => q.GameId);
            entity.HasOne(e => e.CurrentMiniGame)
                .WithOne()
                .HasForeignKey<Core.Models.Game>(e => e.CurrentMiniGameId);
            entity.Ignore(x => x.IsStarted);
            entity.Ignore(x => x.IsFinished);
        });

        modelBuilder.Entity<MiniGameDefinition>(entity =>
        {
            entity.ToTable("MiniGameDefinitions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DefinitionJsonData).HasColumnType("TEXT");
        });

        modelBuilder.Entity<MiniGameInstance>(entity =>
        {
            entity.ToTable("MiniGameInstances");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Game)
                .WithMany(e => e.MiniGames)
                .HasForeignKey(e => e.GameId);
            entity.HasMany(e => e.PlayerScores)
                .WithOne(e => e.MiniGameInstance)
                .HasForeignKey(e => e.MiniGameInstanceId);
            entity.Property(e => e.StateJsonData).HasColumnType("TEXT");

        });

        modelBuilder.Entity<MiniGameInstanceScore>(entity =>
        {
            entity.ToTable("MiniGameInstanceScores");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.MiniGameInstance).WithMany(e => e.PlayerScores).HasForeignKey(e => e.MiniGameInstanceId);
            entity.HasOne(e => e.Player).WithMany(e => e.Scores).HasForeignKey(e => e.PlayerId);
        });
    }

}
