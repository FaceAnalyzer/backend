using FluentAssertions;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Reactions;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests.UseCases.Reactions;

public class CreateReactionUseCaseTests
{
    [Fact(DisplayName = "Successfully Store new reaction")]
    public async Task Should_Store_Reaction_Into_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<CreateReactionUseCase>("CreateReactionUseCaseTests",
                new ReactionMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Add(project);


        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Add(experiment);
        
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
        
        
        var request = new CreateReactionCommand(stimuli.Id, "ExampleName", emotionsList);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert

        var savedReaction = dbContext.Reactions
            .IgnoreQueryFilters()
            .FirstOrDefault();

        // Assertion
        Assert.NotNull(savedReaction);
        Assert.Equal(stimuli.Id, savedReaction.StimuliId);
        Assert.Equal("ExampleName", savedReaction.ParticipantName);

        // Cleanup
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when stimuli does not exists")]
    public async Task? Should_Fail_Storing_Reaction_NotExistingStimuli()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<CreateReactionUseCase>("CreateReactionUseCaseTests",
                new ReactionMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        
        var emotionDictionary = new Dictionary<EmotionType, double>();
        emotionDictionary.Add(EmotionType.Anger, 0.5);
        var emotions1 = new EmotionReading(1000, emotionDictionary);
        var emotions2 = new EmotionReading(1001, emotionDictionary);
        var emotionsList = new List<EmotionReading>();
        emotionsList.Add(emotions1);
        emotionsList.Add(emotions2);
        
        
        var request = new CreateReactionCommand(128, "ExampleName", emotionsList);

        // Act
        var act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        var errorAssertion = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();

        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.StimuliId),
                $"no stimuli with this id ({request.StimuliId}) was found")
            .Build();

        errorAssertion.And.Message.Should().Be(exception.Message);
    }
}