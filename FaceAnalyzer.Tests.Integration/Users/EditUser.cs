using System.Net;
using System.Net.Http.Json;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Users;

public class EditUser
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public EditUser(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Admin can successfully edit a user")]
    public async Task AdminCanEditUser()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        // Create a user to edit
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

        // Prepare the edit user DTO
        var editUserDto = new EditUserDto(
            Name : "new name",
            Surname : "new surname",
            Email : "newemail@gmail.com",
            Username : "newusername",
            ContactNumber : "+393456543444",
            Role : UserRole.Admin
        );

        // Act
        var putResponse = await httpClient.PutAsJsonAsync($"users/{newUser.Id}", editUserDto);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "Editing a non-existing user returns 400")]
public async Task EditNonExistingUserReturns400()
{
    // Arrange
    _fixture
        .EnableAuthentication()
        .AddDefaultPrincipal(1, UserRole.Admin)
        .Build();

    await _fixture.StartHost();
    var httpClient = _fixture.GetClient();

    // Prepare the edit user DTO
    var editUserDto = new EditUserDto(
        Name : "new name",
        Surname : "new surname",
        Email : "newemail@gmail.com",
        Username : "newusername",
        ContactNumber : "+393456543444",
        Role : UserRole.Admin
    );

    // Act
    // Try to edit a user that does not exist
    var putResponse = await httpClient.PutAsJsonAsync($"users/{9999}", editUserDto); // Assuming 9999 is a non-existing user ID

    // Assert
    putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}
}