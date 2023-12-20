using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Reactions;

public class GetReactions
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetReactions(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "user can get all reactions")]
    public async Task UserGetAllReactions()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "User can successfully get a reaction by ID")]
    public async Task UserGetReactionById()
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
        var getResponse = await httpClient.GetAsync($"reactions/{reaction.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "User can successfully get a emotion of a reaction by reactionID")]
    public async Task UserGetReactionEmotionById()
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
        
        var reaction = new Reaction
        {
            ParticipantName = "ExampleName",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);
        
        // Create a Reaction to get
        var emotion = new Emotion
        {
            Value = 50,
            EmotionType = EmotionType.Happiness,
            ReactionId = reaction.Id,
            TimeOffset = 405
        };
        dbContext.Emotions.Add(emotion);
        await dbContext.SaveChangesAsync();

        reaction.Should().NotBeNull();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions/{reaction.Id}/emotions");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "User can successfully export emotions of a reaction by reactionID")]
    public async Task UserExportReactionEmotionById()
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
        
        // Create a Reaction to get
        var emotion = new Emotion
        {
            Value = 50,
            EmotionType = EmotionType.Happiness,
            ReactionId = reaction.Id,
            TimeOffset = 405
        };
        dbContext.Emotions.Add(emotion);
        await dbContext.SaveChangesAsync();

        reaction.Should().NotBeNull();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions/{reaction.Id}/emotions/export");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "Getting a non-existing reaction returns 404")]
    public async Task GetNonExistingReactionReturns404()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to get a reaction that does not exist
        var getResponse = await httpClient.GetAsync($"reactions/{9999}"); // Assuming 9999 is a non-existing reaction ID

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}