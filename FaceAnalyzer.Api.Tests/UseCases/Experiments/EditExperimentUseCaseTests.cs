using FluentAssertions;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Experiments;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FaceAnalyzer.Api.Tests.UseCases.Experiments;

public class EditExperimentUseCaseTests
{
    [Fact(DisplayName = "Successfully Edit an existing experiment")]
    public async Task Should_Edit_Experiment_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<EditExperimentUseCase>("EditExperimentUseCaseTests",
                new ExperimentMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        // Create a new project
        var newProject = new Project { Name = "New Project" };
        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync();
        // Create a new experiment
        var newExperiment = new Experiment
        {
            Description = "New Experiment",
            Name = "New Experiment",
            ProjectId = newProject.Id
        };
        dbContext.Experiments.Add(newExperiment);
        await dbContext.SaveChangesAsync();

        var newName = "Edited Experiment";
        var newDescription = "Edited Experiment Description";
        var request = new EditExperimentCommand(newExperiment.Id, newName, newDescription, newProject.Id);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        var editedExperiment = await dbContext.Experiments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == newExperiment.Id);

        // Assertion
        Assert.NotNull(editedExperiment);
        Assert.Equal(editedExperiment.Name, newName);
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when experiment does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Experiment_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<EditExperimentUseCase>("EditExperimentUseCaseTests",
                new ExperimentMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no experiment with Id 999
        var nonExistingExperimentId = 999;
        var newName = "Edited Experiment";
        var newDescription = "Edited Experiment Description";
        var request = new EditExperimentCommand(nonExistingExperimentId, newName, newDescription, 1);
        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no experiment with this id ({request.Id}) was found");
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when project does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Project_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<EditExperimentUseCase>("EditExperimentUseCaseTests",
                new ExperimentMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        // Create a new project
        var newProject = new Project { Name = "New Project" };
        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync();
        // Create a new experiment
        var newExperiment = new Experiment
        {
            Description = "New Experiment",
            Name = "New Experiment",
            ProjectId = newProject.Id
        };
        dbContext.Experiments.Add(newExperiment);
        // Assuming there is an experiment with Id 1
        var existingExperimentId = newExperiment.Id;
        // Assuming there is no project with Id 999
        var nonExistingProjectId = 999;
        var newName = "Edited Experiment";
        var newDescription = "Edited Experiment Description";
        var request = new EditExperimentCommand(existingExperimentId, newName, newDescription, nonExistingProjectId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"ProjectId: no project with this id ({request.ProjectId}) was found");
    }
}