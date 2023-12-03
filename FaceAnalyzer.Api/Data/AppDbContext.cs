using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Data;

public class AppDbContext : DbContext
{
    private readonly SecurityContext _securityContext;

    public AppDbContext(DbContextOptions<AppDbContext> options, SecurityContext securityContext) : base(options)
    {
        _securityContext = securityContext;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username, u.DeletedAt })
            .IsUnique();

        modelBuilder.AddAutoUpdatedAt();
        modelBuilder.AddAutoCreatedAt();

        AddUsersQueryFilters(modelBuilder);
        AddProjectsQueryFilters(modelBuilder);
        AddExperimentsQueryFilters(modelBuilder);
        AddStimuliQueryFilters(modelBuilder);
        AddReactionsQueryFilters(modelBuilder);
        AddEmotionsQueryFilters(modelBuilder);
        AddNotesQueryFilters(modelBuilder);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : IDeletable
    {
        entity.Delete();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<Stimuli> Stimuli { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Emotion> Emotions { get; set; }
    
    public DbSet<Note> Notes { get; set; }


    #region QueryFilters

    private void AddProjectsQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasQueryFilter(
                p =>
                    !p.DeletedAt.HasValue
                    &&
                    (_securityContext.Principal.Role == UserRole.Admin ||
                     p.Users.Any(u => u.Id == _securityContext.Principal.Id))
            );
    }

    private void AddExperimentsQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Experiment>()
            .HasQueryFilter(
                e =>
                    !e.DeletedAt.HasValue
                    &&
                    (_securityContext.Principal.Role == UserRole.Admin ||
                     e.Project.Users.Any(u => u.Id == _securityContext.Principal.Id))
            );
    }

    private void AddStimuliQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stimuli>()
            .HasQueryFilter(
                s =>
                    !s.DeletedAt.HasValue &&
                    (_securityContext.Principal.Role == UserRole.Admin ||
                     s.Experiment.Project.Users.Any(u => u.Id == _securityContext.Principal.Id))
            );
    }

    private void AddReactionsQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reaction>()
            .HasQueryFilter(
                r =>
                    !r.DeletedAt.HasValue &&
                    (_securityContext.Principal.Role == UserRole.Admin ||
                     r.Stimuli.Experiment.Project.Users.Any(u => u.Id == _securityContext.Principal.Id))
            );
    }

    private void AddEmotionsQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emotion>()
            .HasQueryFilter(
                e =>
                    !e.DeletedAt.HasValue &&
                    (_securityContext.Principal.Role == UserRole.Admin ||
                     e.Reaction.Stimuli.Experiment.Project.Users.Any(u => u.Id == _securityContext.Principal.Id))
            );
    }

    private void AddNotesQueryFilters(ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<Note>()
             .HasQueryFilter(
                 e =>
                     !e.DeletedAt.HasValue &&
                     (_securityContext.Principal.Role == UserRole.Admin ||
                      e.Experiment.Project.Users.Any(u => u.Id == _securityContext.Principal.Id))
                     );
     }

    private void AddUsersQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasQueryFilter(e => !e.DeletedAt.HasValue);
    }

    #endregion
}