using System.Net;
using System.Net.Http.Json;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Auth;

public class LoginTests : IClassFixture<TestHostFixture>
{
    async Task CreateUser(TestHostFixture fixture, string username, string password)
    {
        var dbContext = fixture.GetService<AppDbContext>();
        var user = new User
        {
            Name = username,
            Role = UserRole.Researcher,
            Email = "",
            Password = password,
            Surname = "",
            Username = username
        };
        var securityContext = fixture.GetService<SecurityContext>();
        user.Password = securityContext.Hash(user.Password);
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    [Fact(DisplayName = "Should return 200 OK when user is authenticated")]
    public async Task Should_Return_200_OK_When_User_Is_Authenticated()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture.Build();

        var username = "SUT User" + Guid.NewGuid();
        var password = "SUT Password";
        await CreateUser(fixture, username, password);


        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new LoginRequest(username, password);
        var response = await httpClient.PostAsJsonAsync("/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
    }
    [Fact(DisplayName = "Should return 400 Bad Request when username does not exist")]
    public async Task Should_Return_400_When_Username_Does_Not_Exist()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture.Build();

        var username = "NonExistentUser";
        var password = "SUT Password";

        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new LoginRequest(username, password);
        var response = await httpClient.PostAsJsonAsync("/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [Fact(DisplayName = "Should return 400 Bad Request when password is incorrect")]
    public async Task Should_Return_400_When_Password_Is_Incorrect()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        var username = "SUT User" + Guid.NewGuid();
        var password = "SUT Password";
        await CreateUser(fixture, username, password);

        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new LoginRequest(username, "IncorrectPassword");
        var response = await httpClient.PostAsJsonAsync("/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}