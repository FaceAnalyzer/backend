using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Tests.Integration.Users;

public class DeleteUserTests
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public DeleteUserTests(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Admin can successfully delete a user")]
    public async Task AdminDeleteUser()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        // Create a user to delete
        var newUser = new User
        {
            Name = "name",
            Surname = "surname",
            Email = "example1@gmail.com",
            Username = "username1",
            Password = "Password123",
            ContactNumber = "+393456543333",
            Role = UserRole.Researcher
        };

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        newUser.Should().NotBeNull();

        // Act
        // Delete the user
        var deleteResponse = await httpClient.DeleteAsync($"users/{newUser.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Check that the user has been deleted
        var deletedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == newUser.Id);
        deletedUser.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deleting a non-existing user returns 400")]
    public async Task DeleteNonExistingUserReturns400()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to delete a user that does not exist
        var deleteResponse = await httpClient.DeleteAsync($"users/{9999}"); // Assuming 9999 is a non-existing user ID

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}