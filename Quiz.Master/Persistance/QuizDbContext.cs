using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance.Converters;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Persistance;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Models.Game> Games { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<AnswerSelection> AnswerSelections { get; set; } = null!;
    public DbSet<GameScore> GameScores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DeviceId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Models.Game>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasMany(e => e.Questions)
                  .WithOne(q => q.Game)
                  .HasForeignKey(q => q.GameId);
            entity.HasMany(e => e.Players)
                  .WithOne(q => q.Game)
                  .HasForeignKey(q => q.GameId);
            entity.HasOne(e => e.GameScore)
                  .WithOne(gs => gs.Game)
                  .HasForeignKey<GameScore>(gs => gs.GameId);
            entity.Ignore(x => x.IsStarted);
            entity.Ignore(x => x.IsFinished);
            entity.Property(e => e.Rounds).HasConversion(new GameRoundConverter());
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Content).IsRequired();
            entity.HasOne(e => e.CorrectAnswer)
                  .WithOne()
                  .HasForeignKey<Question>(e => e.CorrectAnswerId);
            entity.HasMany(e => e.Answers)
                  .WithOne(a => a.Question)
                  .HasForeignKey(a => a.QuestionId);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
        });

        modelBuilder.Entity<AnswerSelection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AnsweredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.Answer)
                  .WithMany()
                  .HasForeignKey(e => e.AnswerId);
            entity.HasOne(e => e.Player)
                  .WithMany()
                  .HasForeignKey(e => e.PlayerId);
        });

        modelBuilder.Entity<GameScore>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ScoreValue).IsRequired();
            entity.HasOne(e => e.Player)
                  .WithOne()
                  .HasForeignKey<GameScore>(e => e.PlayerId);
        });
    }

}
