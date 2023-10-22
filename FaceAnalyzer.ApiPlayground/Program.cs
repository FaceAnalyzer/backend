using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<FormOptions>(opt =>
{
    // configurations for file sizes and limits
    // opt.MultipartBodyLengthLimit = 188001;
    // opt.MemoryBufferThreshold = 188001;
});
var app = builder.Build();

var tmp = Path.GetTempPath();
Console.WriteLine(tmp);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();