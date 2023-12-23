using System.Net;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Reactions;

public class CreateReaction :  IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateReaction(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully create a Reaction")]
    public async Task UserCreateReaction()
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

        var emotionDictionary = new Dictionary<EmotionType, double>();
        emotionDictionary.Add(EmotionType.Anger, 0.5);
        var emotions1 = new EmotionReading(1000, emotionDictionary);
        var emotions2 = new EmotionReading(1001, emotionDictionary);
        var emotionsList = new List<EmotionReading>();
        emotionsList.Add(emotions1);
        emotionsList.Add(emotions2);


        //Act
        var dto = new CreateReactionDto(stimuli.Id, "ExampleName", emotionsList);

        var response = await httpClient.PostAsJsonAsync(
            $"reactions",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

}