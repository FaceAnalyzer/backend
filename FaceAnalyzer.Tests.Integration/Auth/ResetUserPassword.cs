using System.Net;
using System.Net.Http.Json;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Auth;

public class ResetUserPassword
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
    
    [Fact(DisplayName = "Should return 200 OK when password is reset by admin")]
    public async Task Should_Return_200_When_Password_Is_Reset_By_Admin()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture
            .AddDefaultPrincipal(1, UserRole.Admin)
            .EnableAuthentication()
            .Build();
        

        var username = "SUT User";
        var password = "SUT Password";
        await CreateUser(fixture, username, password);

        var dbContext = fixture.GetService<AppDbContext>();
        var sutUser = dbContext.Users.First(u => u.Username == username);
        var newPassword = "NewPassword";

        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new ResetUserPasswordDto(sutUser.Id, newPassword);
        var response = await httpClient.PatchAsJsonAsync("/auth/reset-user-password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact(DisplayName = "Should return 403 Forbidden when non-admin tries to reset password")]
    public async Task Should_Return_403_When_Non_Admin_Tries_To_Reset_Password()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture
            .AddDefaultPrincipal(1, UserRole.Researcher) // Non-admin user
            .EnableAuthentication()
            .Build();

        var username = "SUT User";
        var password = "SUT Password";
        await CreateUser(fixture, username, password);

        var user = fixture.GetService<AppDbContext>().Users.First(u => u.Username == username);
        var newPassword = "NewPassword";

        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new ResetUserPasswordDto(user.Id, newPassword);
        var response = await httpClient.PatchAsJsonAsync("/auth/reset-user-password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    [Fact(DisplayName = "Should return 400 Bad Request when user does not exist")]
    public async Task Should_Return_400_When_User_Does_Not_Exist()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture
            .AddDefaultPrincipal(1, UserRole.Admin) // Admin user
            .EnableAuthentication()
            .Build();

        var nonExistentUserId = 999; // Non-existent user ID
        var newPassword = "NewPassword";

        await fixture.StartHost();
        var httpClient = fixture.GetClient();

        // Act
        var request = new ResetUserPasswordDto(nonExistentUserId, newPassword);
        var response = await httpClient.PatchAsJsonAsync("/auth/reset-user-password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "Should return 200 OK when password is reset by the user himself")]
    public async Task User_reset_own_password()
    {
        // Arrange
        var fixture = new TestHostFixture();
        fixture
            .AddDefaultPrincipal(1, UserRole.Researcher)
            .EnableAuthentication()
            .Build();
        
        var username = "SUT User";
        var password = "SUT Password";
        await CreateUser(fixture, username, password);
        


        await fixture.StartHost();
        var httpClient = fixture.GetClient();
        var newPassword = "NewPassword";
        
        // Act
        var request = new ResetMyPasswordDto(newPassword);
        var response = await httpClient.PatchAsJsonAsync("/auth/reset-my-password", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    
}