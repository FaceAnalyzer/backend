using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class EditProjectUseCaseTests
{
    [Fact(DisplayName = "Successfully Edit an existing project")]
    public async Task Should_Edit_Project_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<EditProjectUseCase>("EditProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        // Create a new project
        var newProject = new Project { Name = "New Project" };
        dbContext.Projects.Add(newProject);
        await dbContext.SaveChangesAsync();

        var newName = "Edited Project";
        var request = new EditProjectCommand(newProject.Id, newName);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        var editedProject =await dbContext.Projects
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == newProject.Id);

        // Assertion
        Assert.NotNull(editedProject);
        Assert.Equal(editedProject.Name, newName);


    }
    [Fact(DisplayName = "Throw InvalidArgumentsException when project does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_Project_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<EditProjectUseCase>("EditProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no project with Id 999
        var nonExistingProjectId = 999;
        var newName = "Edited Project";
        var request = new EditProjectCommand(nonExistingProjectId, newName);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Project with Id: {nonExistingProjectId} does not exist");
    }
}