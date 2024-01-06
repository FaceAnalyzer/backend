using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Emotions;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Emotions;

public class CreateEmotionUseCaseTests
{
    [Fact(DisplayName = "Successfully Store new emotion")]
    public async Task Should_Store_Emotion_Into_DB()
    {
        var services = new IsolatedUseCaseTestServices<CreateEmotionUseCase>("CreateEmotionUseCaseTest",
            new EmotionMappingProfile());
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

        var reaction = new Reaction
        {
            ParticipantName = "Fake participant name",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);
        dbContext.SaveChanges();

        var name = "Dummy Experiment";
        var request = new CreateEmotionCommand(0.05, 5000, EmotionType.Anger, reaction.Id);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        var savedEmotion = dbContext.Emotions
            .IgnoreQueryFilters()
            .FirstOrDefault();

        Assert.NotNull(savedEmotion);
        Assert.Equal(savedEmotion.EmotionType, EmotionType.Anger);
        Assert.Equal(savedEmotion.ReactionId, reaction.Id);
        Assert.Equal(savedEmotion.Value, 0.05);
        Assert.Equal(savedEmotion.TimeOffset, 5000);
        
    }

    [Fact(DisplayName = "Should throw Invalid Argument exception when reaction does not exists")]
    public async Task? Should_Fail_Storing_Emotion()
    {
        var services = new IsolatedUseCaseTestServices<CreateEmotionUseCase>("CreateEmotionUseCaseTest",
            new EmotionMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        
        var request = new CreateEmotionCommand(0.05,5000, EmotionType.Anger, 999);

        // Act
        var act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        var errorAssertion = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();

        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ReactionId),
                $"no reaction with this id ({request.ReactionId}) was found")
            .Build();

        errorAssertion.And.Message.Should().Be(exception.Message);
    }
    
}