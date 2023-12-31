using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.ConfigureSwagger();
var config = new AppConfiguration();

builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);
builder.Services.AddBusinessModels();
builder.Services.AddDbContexts(config.ConnectionStrings.AppDatabase, config.ConnectionStrings.DbVersion);
builder.Services.AddAppAuthentication(config);
builder.Services.AddMappers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR();
builder.Services.AddScoped<ErrorHandlerMiddleware>();

#endregion


var app = builder.Build();


#region Pipeline


app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSwagger(opt => opt.SerializeAsV2 = false);
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyOrigin();
    opt.AllowAnyMethod();
});
app.UseAuthentication();
app.UseMiddleware<SetSecurityPrincipalMiddleware>();
app.UseAuthorization();

app.MapControllers();


app.Run();

#endregion