using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.UseCases.Experiments;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Experiments;

public class CreateExperimentUseCaseTest : UseCaseTestBase<CreateExperimentUseCase>
{
    [Fact(DisplayName = "Successfully Store new experiment")]
    public async Task Should_Store_Experiment_Into_DB()
    {
        
        // Arrange
        CleanDatabase();
        var project = new Project
        {
            Name = "Dummy Project"
        };
        DbContext.Add(project);
        DbContext.SaveChanges();
        
        var name = "Dummy Experiment";
        var request = new CreateExperimentCommand(name, "", project.Id);

        // Act
        var result = await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        var savedExperiment = DbContext.Experiments
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        Assert.NotNull(savedExperiment);
        Assert.Equal(savedExperiment.Name, name);
        Assert.Equal(savedExperiment.ProjectId, project.Id);
        
        

    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when project does not exists")]
    public async Task? Should_Fail_Storing_Experiment()
    {

        
        CleanDatabase();
        
        var name = "Dummy Experiment";
        var request = new CreateExperimentCommand(name, "", 132);

        // Act
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        var errorAssertion  = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();
        
        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ProjectId),
                $"no project with this id ({request.ProjectId}) was found")
            .Build();
        
        errorAssertion.And.Message.Should().Be(exception.Message);
    }

    public CreateExperimentUseCaseTest(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
}