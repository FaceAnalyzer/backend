using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Tests.Integration.Reactions;

public class DeleteReaction
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public DeleteReaction(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully delete a reaction")]
    public async Task UserDeleteReaction()
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

        // Create a Reaction to get
        var reaction = new Reaction
        {
            ParticipantName = "ExampleName",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);
        await dbContext.SaveChangesAsync();

        reaction.Should().NotBeNull();

        // Act
        // Delete the reaction
        var deleteResponse = await httpClient.DeleteAsync($"reactions/{reaction.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Check that the reaction has been deleted
        var deletedReaction = await dbContext.Reactions.FirstOrDefaultAsync(u => u.Id == reaction.Id);
        deletedReaction.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deleting a non-existing reaction gives response 400")]
    public async Task DeleteNonExistingReactionReturns400()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to delete a user that does not exist
        var deleteResponse = await httpClient.DeleteAsync($"reactions/{9999}"); // Assuming 9999 is a non-reaction ID

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}