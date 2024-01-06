using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Tests.Integration.StimuliTests;

public class DeleteStimuli : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public DeleteStimuli(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "User can successfully delete a stimulus")]
    public async Task UserDeleteStimulus()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Researcher)
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

        stimuli.Should().NotBeNull();

        // Act
        // Delete the stimulus
        var deleteResponse = await httpClient.DeleteAsync($"stimuli/{stimuli.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Check that the stimulus has been deleted
        var deletedStimulus = await dbContext.Stimuli.FirstOrDefaultAsync(u => u.Id == stimuli.Id);
        deletedStimulus.DeletedAt.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Trying to delete a non existing stimulus return bad request")]
    public async Task NotPossibleToDeleteNonExistingStimulus()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Delete the stimulus
        var deleteResponse = await httpClient.DeleteAsync($"stimuli/{999}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}