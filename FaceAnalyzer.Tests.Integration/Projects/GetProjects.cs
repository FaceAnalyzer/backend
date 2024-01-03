using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace FaceAnalyzer.Tests.Integration.Projects;

public class GetProjects
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetProjects(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully get all projects")]
    public async Task UserGetAllProjects()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();
        
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"projects");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"projects");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<ProjectDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full projects list from the database.
        var dbProjects = dbContext.Projects.ToList();
        foreach (var project2 in response!.Items)
        {
            var dbProject = dbProjects.Find(p =>
                p.Id == project2.Id);
            dbProject.Name.Should().Be(project2.Name);
        }
    }
    
    [Fact(DisplayName = "User can successfully get a project using the name")]
    public async Task UserGetProjectByName()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();
        
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"projects?ProjectName={project.Name}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"projects");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<ProjectDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full projects list from the database.
        var dbProjects = dbContext.Projects.ToList();
        foreach (var project2 in response!.Items)
        {
            var dbProject = dbProjects.Find(p =>
                p.Id == project2.Id);
            dbProject.Name.Should().Be(project2.Name);
        }
    }
    
    [Fact(DisplayName = "User can successfully get a project using the id")]
    public async Task UserGetProjectById()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();
        
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"projects/{project.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"projects/{project.Id}");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };
        
        var responseProject = JsonSerializer.Deserialize<Project>(jsonResponse, jsonOptions);
        
        // Assert
        responseProject.Should().NotBeNull();
        responseProject?.Name.Should().Be(project.Name);
    }

    
    [Fact(DisplayName = "Getting a non-existing project returns 404")]
    public async Task GetNonExistingProjectReturns404()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to get an experiment that does not exist
        var getResponse = await httpClient.GetAsync($"projects/{9999}"); // Assuming 9999 is a non-existing project Id

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}