using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Experiments;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Experiments;

public class DeleteExperimentUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete an existing experiment")]
    public async Task Should_Delete_Experiment_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteExperimentUseCase>("DeleteExperimentUseCaseTests",
                new ExperimentMappingProfile());
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
        await dbContext.SaveChangesAsync();

        var request = new DeleteExperimentCommand(experiment.Id);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        var deletedExperiment = await dbContext.Experiments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == experiment.Id);

        // Assertion
        deletedExperiment.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when experiment does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Experiment_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteExperimentUseCase>("DeleteExperimentUseCaseTests",
                new ExperimentMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no experiment with Id 999
        var nonExistingExperimentId = 999;
        var request = new DeleteExperimentCommand(nonExistingExperimentId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no experiment with this id ({request.Id}) was found");
    }
    
}