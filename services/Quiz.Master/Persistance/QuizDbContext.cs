using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Persistance;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Models.Game> Games { get; set; } = null!;
    public DbSet<MiniGameDefinition> MiniGameDefinitions { get; set; } = null!;

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

        modelBuilder.Entity<Models.Game>(entity =>
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
                .HasForeignKey<Models.Game>(e => e.CurrentMiniGameId);
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

        // modelBuilder.Entity<Question>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        //     entity.Property(e => e.Content).IsRequired();
        //     entity.HasOne(e => e.CorrectAnswer)
        //           .WithOne()
        //           .HasForeignKey<Question>(e => e.CorrectAnswerId);
        //     entity.HasMany(e => e.Answers)
        //           .WithOne(a => a.Question)
        //           .HasForeignKey(a => a.QuestionId);
        // });

        // modelBuilder.Entity<Answer>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.Content).IsRequired();
        // });

        // modelBuilder.Entity<AnswerSelection>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.AnsweredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        //     entity.HasOne(e => e.Answer)
        //           .WithMany()
        //           .HasForeignKey(e => e.AnswerId);
        //     entity.HasOne(e => e.Player)
        //           .WithMany()
        //           .HasForeignKey(e => e.PlayerId);
        // });

        // Mini games
        // modelBuilder.Entity<AbcdCategoryMiniGame>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.HasOne(e => e.Game)
        //         .WithMany(e => e.AbcdCategoryMiniGames)
        //         .HasForeignKey(e => e.GameId);
        //     entity.HasMany(e => e.Rounds)
        //         .WithOne(e => e.MiniGame)
        //         .HasForeignKey(e => e.MiniGameId);
        //     entity.HasOne(e => e.CurrentRound)
        //         .WithMany()
        //         .HasForeignKey(e => e.CurrentRoundId);
        // });

        // modelBuilder.Entity<AbcdCategoryMiniGame>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.HasOne(e => e.Game)
        //         .WithMany(e => e.AbcdCategoryMiniGames)
        //         .HasForeignKey(e => e.GameId);
        //     entity.HasMany(e => e.Rounds)
        //         .WithOne(e => e.MiniGame)
        //         .HasForeignKey(e => e.MiniGameId);
        //     entity.HasOne(e => e.CurrentRound)
        //         .WithMany()
        //         .HasForeignKey(e => e.CurrentRoundId);
        // });

    }

}
