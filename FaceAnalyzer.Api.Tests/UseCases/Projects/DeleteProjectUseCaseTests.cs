using FluentAssertions;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class DeleteProjectUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete an existing project")]
    public async Task Should_Delete_Project_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteProjectUseCase>("DeleteProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        // Create a new project
        var newProject = new Project { Name = "New Project" };
        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync();

        var request = new DeleteProjectCommand(newProject.Id);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        var deletedProject = await dbContext.Projects
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == newProject.Id);

        // Assertion
        deletedProject.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when project does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Project_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteProjectUseCase>("DeleteProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no project with Id 999
        var nonExistingProjectId = 999;
        var request = new DeleteProjectCommand(nonExistingProjectId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no project with this id ({request.Id}) was found");
    }
}