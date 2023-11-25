using FluentAssertions;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class CreateProjectUseCaseTests : UseCaseTestBase<CreateProjectUseCase>
{
    [Fact(DisplayName = "Successfully Store new project")]
    public async Task Should_Store_Project_Into_DB()
    {
        // Arrange
        
        var name = "Dummy Project";
        var request = new CreateProjectCommand(name);

        // Act
        var result = await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        
        var savedProject = DbContext.Projects
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        // Assertion
        Assert.NotNull(savedProject);
        Assert.Equal(savedProject.Name, name);
        
        // Cleanup

    }

    public CreateProjectUseCaseTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
}