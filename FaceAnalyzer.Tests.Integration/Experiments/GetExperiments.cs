using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;


namespace FaceAnalyzer.Tests.Integration.Experiments;

public class GetExperiments
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetExperiments(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully get all experiments given a project")]
    public async Task UserGetAllExperiments()
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
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"experiments?ProjectId={project.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"experiments?ProjectId={project.Id}");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<ExperimentDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full experiments list from the database.
        var dbExperiments = dbContext.Experiments.ToList();
        foreach (var experiment2 in response!.Items)
        {
            var dbExperiment = dbExperiments.Find(e =>
                e.Id == experiment2.Id);
            dbExperiment.Name.Should().Be(experiment2.Name);
            dbExperiment.ProjectId.Should().Be(experiment2.ProjectId);
            dbExperiment.Description.Should().Be(experiment2.Description);
        }
    }
    
    [Fact(DisplayName = "User can successfully get experiments using the Id")]
    public async Task UserGetExperimentById()
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
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"experiments/{experiment.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"experiments/{experiment.Id}");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };
        
        var response = JsonSerializer.Deserialize<Experiment>(jsonResponse, jsonOptions);
        
        // Assert
        response.Should().NotBeNull();
        response?.Name.Should().Be(experiment.Name);
        response?.ProjectId.Should().Be(experiment.ProjectId);
        response?.Description.Should().Be(experiment.Description);
    }

    
    [Fact(DisplayName = "Getting a non-existing experiment returns 404")]
    public async Task GetNonExistingExperimentReturns404()
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
        var getResponse = await httpClient.GetAsync($"experiments/{9999}"); // Assuming 9999 is a non-existing experiment Id

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}