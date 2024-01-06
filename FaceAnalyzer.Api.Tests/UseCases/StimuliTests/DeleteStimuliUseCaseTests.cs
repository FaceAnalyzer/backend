using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.StimuliUseCases;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.StimuliTests;

public class DeleteStimuliUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete an existing stimuli")]
    public async Task Should_Delete_Stimuli_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteStimuliUseCase>("DeleteStimuliUseCaseTests",
                new StimuliMappingProfile());
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
        
        var request = new DeleteStimuliCommand(stimuli.Id);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        var deletedStimuli = await dbContext.Stimuli
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == stimuli.Id);

        // Assertion
        deletedStimuli.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when stimuli does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Stimuli_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteStimuliUseCase>("DeleteStimuliUseCaseTests",
                new StimuliMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no stimuli with Id 999
        var nonExistingStimuliId = 999;
        var request = new DeleteStimuliCommand(nonExistingStimuliId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no stimuli with this id ({request.Id}) was found");
    }
}