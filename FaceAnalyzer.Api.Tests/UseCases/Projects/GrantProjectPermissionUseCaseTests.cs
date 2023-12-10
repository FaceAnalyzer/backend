using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class GrantProjectPermissionUseCaseTests
{
    List<string> _userNameList = new List<string>
    {
        "John", "Smith"
    };

    List<string> _projectsNameList = new List<string>
    {
        "Alpha", "Beta"
    };

    async Task Arrange(AppDbContext dbContext, List<string> userNames, List<string> projectNames)
    {
        dbContext.AddRange(
            projectNames.Select(n => new Project
            {
                Name = n
            })
        );
        dbContext.AddRange(
            userNames.Select(n => new User
            {
                Name = n
            })
        );
        await dbContext.SaveChangesAsync();
    }

    [Fact(DisplayName = "Should add users to project if the project exists")]
    public async Task AssignUsersToProject()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<GrantProjectPermissionUseCase>("CreateProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        await Arrange(dbContext, _userNameList, _projectsNameList);

        var user = dbContext.Users
            .AsNoTracking()
            .First();
        var project = dbContext.Projects
            .AsNoTracking()
            .First();

        var request = new GrantProjectPermissionCommand(project.Id, new List<int>
        {
            user.Id
        });
        var result = await useCase.Handle(request, CancellationToken.None);

        var updatedProject = dbContext.Projects
            .Include(p => p.Users)
            .FirstOrDefault();


        updatedProject.Users.Should().NotBeEmpty();
        updatedProject.Users.Should().Contain(u => u.Id == user.Id);
    }

    [Fact(DisplayName = "Should throw invalid argument error when try adding users to project that does not exists")]
    public async Task ThrowErrorIfProjectDoesNotExists()
    {
        var services =
            new IsolatedUseCaseTestServices<GrantProjectPermissionUseCase>("CreateProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        await Arrange(dbContext, _userNameList, _projectsNameList);

        var user = dbContext.Users
            .AsNoTracking()
            .First();

        var projectsCount = dbContext.Projects.Count();

        var request = new GrantProjectPermissionCommand(projectsCount + 12, new List<int>
        {
            user.Id
        });
        var act = async () => await useCase.Handle(request, CancellationToken.None);
        var error = await act.Should().ThrowAsync<InvalidArgumentsException>();

        var invalidArgumentException = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ProjectId),
                $"No project with this id {request.ProjectId} was found")
            .Build();

        error.And.Message.Should().Be(invalidArgumentException.Message);
    }

    [Fact(DisplayName = "Should throw bad request when try adding already assigned users to project")]
    public async Task ThrowErrorWhenUsersAlreadyAdded()
    {
        var services =
            new IsolatedUseCaseTestServices<GrantProjectPermissionUseCase>("CreateProjectUseCaseTests",
                new ProjectMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        await Arrange(dbContext, _userNameList, _projectsNameList);
        var project = dbContext.Projects
            .First();
        var researcher = dbContext.Users.First();

        project.Users.Add(researcher);
        await dbContext.SaveChangesAsync();


        var request = new GrantProjectPermissionCommand(project.Id, new List<int>
        {
            researcher.Id
        });
        var act = async () => await useCase.Handle(request, CancellationToken.None);
        var error = await act.Should().ThrowAsync<ProjectGrantPermissionException>();

        var exception = new ProjectGrantPermissionException(researcher.Id, project.Name);

        error.And.Message.Should().Be(exception.Message);
    }
}