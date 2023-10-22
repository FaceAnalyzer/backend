using FaceAnalyzer.ApiPlayground;
using FaceAnalyzer.ApiPlayground.data;
using FaceAnalyzer.ApiPlayground.Services;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("app");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
    options.UseMySql(connectionString, serverVersion);
    
});

var hangfireConnection = builder.Configuration.GetConnectionString("hangfire");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(hangfireConnection)
);
;

builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<FileWriteTask>();
builder.Services.Configure<FormOptions>(opt =>
{
    // configurations for file sizes and limits
    // opt.MultipartBodyLengthLimit = 188001;
    // opt.MemoryBufferThreshold = 188001;
});
var app = builder.Build();

app.UseCors(p =>
{
    p.AllowAnyMethod();
    p.AllowAnyOrigin();
    p.AllowAnyHeader();
});


app.UseHangfireServer();
app.UseHangfireDashboard();

app.MapControllers();

app.Run();