using FluentAssertions;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class CreateProjectUseCaseTests
{
    [Fact(DisplayName = "Successfully Store new project")]
    public async Task Should_Store_Project_Into_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<CreateProjectUseCase>("CreateProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        var name = "Dummy Project";
        var request = new CreateProjectCommand(name);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert

        var savedProject = dbContext.Projects
            .IgnoreQueryFilters()
            .FirstOrDefault();

        // Assertion
        Assert.NotNull(savedProject);
        Assert.Equal(savedProject.Name, name);

        // Cleanup
    }
}