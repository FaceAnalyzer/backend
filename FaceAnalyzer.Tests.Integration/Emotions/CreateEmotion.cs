using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Emotions;
public class CreateEmotion : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public CreateEmotion(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "user can create an emotion")]
    public async Task UserCreateAnEmmotion()
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
        
        //Act
        var dto = new CreateEmotionDto(0.5, 1800, EmotionType.Anger,
            reaction.Id);
        
        var response = await httpClient.PostAsJsonAsync(
            $"emotions",
            dto
        );
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}