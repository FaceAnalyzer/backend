using FaceAnalyzer.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //   //   var deletableEntities = typeof(IDeletable).Assembly
    //   //           .GetTypes()
    //   //           .Where(t => t.IsClass)
    //   //           .Where(t => t.IsAssignableFrom(typeof(IDeletable)))
    //   //       ;
    //   //
    //   // foreach (var type in deletableEntities)
    //   // {
    //   //   modelBuilder.Entity(type)
    //   //       .HasQueryFilter<IDeletable>( x => x. )
    //   //       // .HasQueryFilter(x => !x.DeletedAt);
    //   //  
    //   // }
    //   //       
    // }

    public DbSet<User> Users { get; set; }
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<Stimuli> Stimuli { get; set; }
    public DbSet<Project> Projects { get; set; }
    
}