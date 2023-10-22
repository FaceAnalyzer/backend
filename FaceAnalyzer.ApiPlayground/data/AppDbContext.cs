using FaceAnalyzer.ApiPlayground.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.ApiPlayground.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<FileUpload> FileUploads { get; set; }
    public DbSet<FileChunk> FileChunks { get; set; }
}