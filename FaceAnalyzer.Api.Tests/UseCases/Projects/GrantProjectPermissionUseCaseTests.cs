using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.UseCases.Projects;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Projects;

public class GrantProjectPermissionUseCaseTests : UseCaseTestBase<GrantProjectPermissionUseCase>
{
    List<string> _userNameList = new List<string>
    {
        "John", "Smith"
    };
    List<string> _projectsNameList = new List<string>
    {
        "Alpha", "Beta"
    };
    
    void Arrange(List<string> userNames, List<string> projectNames)
    {
        DbContext.AddRange(
            projectNames.Select(n => new Project
            {
                Name = n
            })
        );
        DbContext.AddRange(
            userNames.Select(n => new User
            {
                Name = n
            })
        );
        DbContext.SaveChanges();
    }

    [Fact(DisplayName = "Should add users to project if the project exists")]
    public async Task AssignUsersToProject()
    {
        Arrange( _userNameList, _projectsNameList);

        var user = DbContext.Users
            .AsNoTracking()
            .First();
        var project = DbContext.Projects
            .AsNoTracking()
            .First();

        var request = new GrantProjectPermissionCommand(project.Id, new List<int>
        {
            user.Id
        });
        var result = await UseCase.Handle(request, CancellationToken.None);

        var updatedProject = DbContext.Projects
            .Include(p => p.Users)
            .FirstOrDefault();


        updatedProject.Users.Should().NotBeEmpty();
        updatedProject.Users.Should().Contain(u => u.Id == user.Id);
        CleanDatabase();
    }

    [Fact(DisplayName = "Should throw invalid argument error when try adding users to project that does not exists")]
    public async Task ThrowErrorIfProjectDoesNotExists()
    {
        Arrange( _userNameList, _projectsNameList);

        var user = DbContext.Users
            .AsNoTracking()
            .First();

        var projectsCount = DbContext.Projects.Count();

        var request = new GrantProjectPermissionCommand(projectsCount + 12, new List<int>
        {
            user.Id
        });
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        var error = await act.Should().ThrowAsync<InvalidArgumentsException>();

        var invalidArgumentException = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ProjectId),
                $"No project with this id {request.ProjectId} was found")
            .Build();

        error.And.Message.Should().Be(invalidArgumentException.Message);
        CleanDatabase();
    }

    [Fact(DisplayName = "Should throw bad request when try adding already assigned users to project")]
    public async Task ThrowErrorWhenUsersAlreadyAdded()
    {
        Arrange(_userNameList, _projectsNameList);

        var project = DbContext.Projects
            .First();
        var researcher = DbContext.Users.First();

        project.Users.Add(researcher);
        DbContext.SaveChanges();


        var request = new GrantProjectPermissionCommand(project.Id, new List<int>
        {
            researcher.Id
        });
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        var error = await act.Should().ThrowAsync<ProjectGrantPermissionException>();

        var exception = new ProjectGrantPermissionException(researcher.Id, project.Name);

        error.And.Message.Should().Be(exception.Message);
        CleanDatabase();
    }

    public GrantProjectPermissionUseCaseTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
}