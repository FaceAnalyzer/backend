using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Reactions;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Reactions;

public class DeleteReactionUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete an existing reaction")]
    public async Task Should_Delete_Reaction_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteReactionUseCase>("DeleteReactionUseCaseTests",
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

        var emotions = new List<Emotion>();

        var newReaction = new Reaction
        {
            StimuliId = stimuli.Id,
            ParticipantName = "Dummy participant",
            Emotions = emotions
        };
        dbContext.Reactions.Add(newReaction);
        await dbContext.SaveChangesAsync();

        var request = new DeleteReactionCommand(newReaction.Id);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        var deletedReaction = await dbContext.Reactions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == newReaction.Id);

        // Assertion
        deletedReaction.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when reaction does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Reaction_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteReactionUseCase>("DeleteReactionUseCaseTests",
                new ReactionMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no reaction with Id 999
        var nonExistingReactionId = 999;
        var request = new DeleteReactionCommand(nonExistingReactionId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no reaction with this id ({request.Id}) was found");
    }
    
}