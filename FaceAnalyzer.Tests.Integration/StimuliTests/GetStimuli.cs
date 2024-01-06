using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;


namespace FaceAnalyzer.Tests.Integration.StimuliTests;

public class GetStimuli
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetStimuli(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully get all the stimuli given an experiment")]
    public async Task UserGetAllStimuli()
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
        
        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"stimuli?ExperimentId={experiment.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"stimuli?ExperimentId={experiment.Id}");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<StimuliDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full stimuli list from the database.
        var dbStimuli = dbContext.Stimuli.ToList();
        foreach (var stimulus2 in response!.Items)
        {
            var dbStimulus = dbStimuli.Find(s =>
                s.Id == stimulus2.Id);
            dbStimulus.Link.Should().Be(stimulus2.Link);
            dbStimulus.Name.Should().Be(stimulus2.Name);
            dbStimulus.ExperimentId.Should().Be(experiment.Id);
            dbStimulus.Description.Should().Be(stimulus2.Description);
        }
    }
    
    [Fact(DisplayName = "User can successfully get stimulus using the Id")]
    public async Task UserGetStimulusById()
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
        
        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);
        
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"stimuli/{stimuli.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"stimuli/{stimuli.Id}");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };
        
        var response = JsonSerializer.Deserialize<Stimuli>(jsonResponse, jsonOptions);
        
        // Assert
        response.Should().NotBeNull();
        response?.Link.Should().Be(stimuli.Link);
        response?.Name.Should().Be(stimuli.Name);
        response?.ExperimentId.Should().Be(stimuli.ExperimentId);
        response?.Description.Should().Be(stimuli.Description);
    }

    
    [Fact(DisplayName = "Getting a non-existing stimulus returns 404")]
    public async Task GetNonExistingStimulusReturns404()
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
        var getResponse = await httpClient.GetAsync($"stimuli/{9999}"); // Assuming 9999 is a non-existing stimulus Id

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}