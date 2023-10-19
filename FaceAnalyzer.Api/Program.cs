using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Shared;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
     option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
     option.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type=ReferenceType.SecurityScheme,
                     Id="Bearer"
                 }
             },
             new string[]{}
         }
     });
     
 
} );
var config = new AppConfiguration();

builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);
builder.Services.AddBusinessModels();
builder.Services.AddDbContexts(config.ConnectionStrings.AppDatabase);
builder.Services.AddAppAuthentication(config);
builder.Services.AddHttpContextAccessor();
#endregion


var app = builder.Build();


#region Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion