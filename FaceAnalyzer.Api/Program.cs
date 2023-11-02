using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();
builder.Services.AddSwagger();
var config = new AppConfiguration();

builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);
builder.Services.AddBusinessModels();
builder.Services.AddDbContexts(config.ConnectionStrings.AppDatabase, config.ConnectionStrings.DbVersion);
builder.Services.AddAppAuthentication(config);
builder.Services.AddMappers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR();

#endregion


var app = builder.Build();


#region Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyOrigin();
    opt.AllowAnyMethod();
});
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SetSecurityPrincipalMiddleware>();

app.MapControllers();

app.Run();

#endregion