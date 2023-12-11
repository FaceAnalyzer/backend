using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.StimuliUseCases;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.StimuliTests;

public class CreateStimuliUseCaseTests
{
    [Fact(DisplayName = "Handle method should add valid Stimuli to DB and return DTO")]
    public async Task Handle_ValidRequest_AddsStimuliToDbAndReturnsDto()
    {
        // Arrange
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<CreateStimuliUseCase>(
                "CreateStimuliUseCaseTests",
                new StimuliMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Add(project);
        await dbContext.SaveChangesAsync();
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Add(experiment);
        await dbContext.SaveChangesAsync();
        var stimuliCommand = new CreateStimuliCommand(
            "https://test.com",
            "Test Description",
            "Test Link",
            1);


        // Act
        var result = await useCase.Handle(stimuliCommand, CancellationToken.None);

        // Assert
        var stimuliInDb =
            await dbContext.Stimuli.FirstOrDefaultAsync(s => s.ExperimentId == stimuliCommand.ExperimentId);
        Assert.NotNull(stimuliInDb);
        Assert.Equal(stimuliCommand.Description, stimuliInDb.Description);
        Assert.Equal(stimuliCommand.Link, stimuliInDb.Link);
        Assert.Equal(stimuliCommand.Name, stimuliInDb.Name);
        Assert.Equal(stimuliCommand.ExperimentId, result.ExperimentId);
        Assert.Equal(stimuliCommand.Description, result.Description);
        Assert.Equal(stimuliCommand.Link, result.Link);
        Assert.Equal(stimuliCommand.Name, result.Name);
    }
    [Fact(DisplayName = "Handle method should throw InvalidArgumentsException when ExperimentId does not exist")]
    public async Task Handle_InvalidExperimentId_ThrowsInvalidArgumentsException()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<CreateStimuliUseCase>(
                "CreateStimuliUseCaseTests",
                new StimuliMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        var stimuliCommand = new CreateStimuliCommand(
            "https://test.com",
            "Test Description",
            "Test Link",
            1);

        // Act
        Func<Task> act = async () => await useCase.Handle(stimuliCommand, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidArgumentsException>(act);
        Assert.Contains($"no experiment with this id ({stimuliCommand.ExperimentId}) was found", exception.Message);
    }
}