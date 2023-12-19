using System.Net;
using System.Linq;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FaceAnalyzer.Tests.Integration.Users;

public class GetUsers
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetUsers(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Admin can successfully get all users")]
    public async Task AdminGetAllUsers()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        var getResponse = await httpClient.GetAsync($"users");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact(DisplayName = "Admin can successfully get users by ProjectId")]
    public async Task AdminGetUsersByProjectId()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        var getResponse = await httpClient.GetAsync($"users?ProjectId={1}"); // Assuming 1 is a valid ProjectId

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "Admin can successfully get users by Role")]
    public async Task AdminGetUsersByRole()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        var getResponse = await httpClient.GetAsync($"users?Role={UserRole.Researcher}"); // Assuming UserRole.Researcher is a valid Role

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact(DisplayName = "Admin can successfully get a user by ID")]
    public async Task AdminGetUserById()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        // Create a user to get
        var newUser = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "example1@gmail.com",
            Username = "username1" + Guid.NewGuid(),
            Password = "Password123",
            ContactNumber = "+393456543333",
            Role = UserRole.Researcher
        };

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        newUser.Should().NotBeNull();

        // Act
        var getResponse = await httpClient.GetAsync($"users/{newUser.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact(DisplayName = "Getting a non-existing user returns 404")]
    public async Task GetNonExistingUserReturns404()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to get a user that does not exist
        var getResponse = await httpClient.GetAsync($"users/{9999}"); // Assuming 9999 is a non-existing user ID

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}