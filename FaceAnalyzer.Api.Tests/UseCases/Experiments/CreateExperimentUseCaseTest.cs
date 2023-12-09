using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Experiments;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Experiments;

public class CreateExperimentUseCaseTest
{
    [Fact(DisplayName = "Successfully Store new experiment")]
    public async Task Should_Store_Experiment_Into_DB()
    {
        var services = new IsolatedUseCaseTestServices<CreateExperimentUseCase>("CreateExperimentUseCaseTest",
            new ExperimentMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Add(project);
        dbContext.SaveChanges();

        var name = "Dummy Experiment";
        var request = new CreateExperimentCommand(name, "", project.Id);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        var savedExperiment = dbContext.Experiments
            .IgnoreQueryFilters()
            .FirstOrDefault();

        Assert.NotNull(savedExperiment);
        Assert.Equal(savedExperiment.Name, name);
        Assert.Equal(savedExperiment.ProjectId, project.Id);
    }

    [Fact(DisplayName = "Should throw Invalid Argument exception when project does not exists")]
    public async Task? Should_Fail_Storing_Experiment()
    {
        var services = new IsolatedUseCaseTestServices<CreateExperimentUseCase>("CreateExperimentUseCaseTest",
            new ExperimentMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        var name = "Dummy Experiment";
        var request = new CreateExperimentCommand(name, "", 132);

        // Act
        var act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        var errorAssertion = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();

        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ProjectId),
                $"no project with this id ({request.ProjectId}) was found")
            .Build();

        errorAssertion.And.Message.Should().Be(exception.Message);
    }
}