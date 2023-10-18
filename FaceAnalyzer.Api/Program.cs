using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Shared;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = new AppConfiguration();
builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);
builder.Services.AddBusinessModels();
builder.Services.AddDbContexts(config.ConnectionStrings.AppDatabase);

#endregion




var app = builder.Build();


#region Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion