using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FaceAnalyzer.Tests.Integration.Projects;

public class RevokeProjectPermissionsTest
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public RevokeProjectPermissionsTest(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    private async Task<Project> AddProject()
    {
        var dbContext = _fixture.GetService<AppDbContext>();

        var result = await dbContext.Projects.AddAsync(new Project
        {
            Name = "SUT Project"
        });

        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    private async Task<List<User>> AddUsers()
    {
        var dbContext = _fixture.GetService<AppDbContext>();
        var users = new List<User>
        {
            new()
            {
                Name = "SUT Researcher",
                Role = UserRole.Researcher,
            },
            new()
            {
                Name = "SUT Researcher",
                Role = UserRole.Researcher,
            }
        };
        foreach (var user in users)
        {
            await dbContext.Users.AddAsync(user);
        }

        await dbContext.SaveChangesAsync();
        return await dbContext.Users.ToListAsync();
    }

    [Fact(DisplayName = "Revoke permission of project from given researcher successfully")]
    public async Task RevokeProjectPermissionWhenUserAndProjectExits()
    {
        // Arrange: add project and users to db
        _fixture
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();
        var project = await AddProject();
        var users = await AddUsers();
        var researcherId = users.First().Id;


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new GrantRevokeProjectPermissionDto(new List<int>(researcherId));

        var response = await httpClient.PutAsJsonAsync(
            $"projects/{project.Id}/researcher/remove",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var dbContext = _fixture.GetService<AppDbContext>();

        var updatedProject = await dbContext.Projects
            .Include(p => p.Users)
            .IgnoreQueryFilters()
            .FirstAsync(p => p.Id == project.Id);

        updatedProject.Users
            .Should().NotContain(u => u.Id == researcherId);
    }

    [Fact(DisplayName = "Revoke project permission when project does not exists should return BadRequest")]
    public async Task RevokeProjectPermissionProjectDoesNotExits()
    {
        // Arrange: add project and users to db
        _fixture
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        var users = await AddUsers();
        var researcherId = users.First().Id;

        var projectId = -1;
        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new GrantRevokeProjectPermissionDto(new List<int>(researcherId));

        var response = await httpClient.PutAsJsonAsync(
            $"projects/{projectId}/researcher/remove",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var stream = await response.Content.ReadAsStreamAsync();
        var body = await JsonSerializer.DeserializeAsync<ProblemDetails>(stream);
        body.Should().NotBeNull();
        body.Title.Should().Be(nameof(InvalidArgumentsException));
    }

    [Fact(DisplayName = "Revoke project permission when one researcher does not exist should return BadRequest")]
    public async Task RevokeProjectPermissionResearcherDoesNotExits()
    {
        // Arrange: add project and users to db
        _fixture
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        var project = await AddProject();
        var users = await AddUsers();
        var researcherIds = users.Select(u => u.Id).ToList();
        researcherIds.Add(-1); // this will cause the error; 

        var projectId = project.Id;
        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new GrantRevokeProjectPermissionDto(researcherIds);

        var response = await httpClient.PutAsJsonAsync(
            $"projects/{projectId}/researcher/remove",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var stream = await response.Content.ReadAsStreamAsync();
        var body = await JsonSerializer.DeserializeAsync<ProblemDetails>(stream);
        body.Should().NotBeNull();
        body.Title.Should().Be(nameof(ProjectRevokePermissionException));
    }
}