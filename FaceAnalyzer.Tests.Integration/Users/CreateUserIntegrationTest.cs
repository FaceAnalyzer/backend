using System.Net;
using System.Net.Http.Json;
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

namespace FaceAnalyzer.Tests.Integration.Users;

public class CreateUserIntegrationTest
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateUserIntegrationTest(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Create a researcher user successfully")]
    public async Task CreateResearcherUser()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new CreateUserDto("name", "surname", "example1@gmail.com",
            "username1", "Password123", "+393456543333",
            UserRole.Researcher);
        

        var response = await httpClient.PostAsJsonAsync(
            $"users",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dbContext = _fixture.GetService<AppDbContext>();

        var newUser = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto.Email && u.Role == dto.Role);
        
        newUser.Should().NotBeNull();
        newUser.Name.Should().Be(dto.Name);
        newUser.Surname.Should().Be(dto.Surname);
        newUser.Email.Should().Be(dto.Email);
        newUser.Username.Should().Be(dto.Username);
        newUser.ContactNumber.Should().Be(dto.ContactNumber);
        newUser.Role.Should().Be(dto.Role);
    }
    
    [Fact(DisplayName = "Create an Admin user successfully")]
    public async Task CreateAdminUser()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new CreateUserDto("name", "surname", "example2@gmail.com",
            "username2", "Password123", "+393456543332",
            UserRole.Admin);
        

        var response = await httpClient.PostAsJsonAsync(
            $"users",
            dto
        );

        // Assert

        //response.Content..ToString().Should().Be("prova");
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var dbContext = _fixture.GetService<AppDbContext>();

        var newUser = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto.Email && u.Role == dto.Role);
        
        newUser.Should().NotBeNull();
        newUser.Name.Should().Be(dto.Name);
        newUser.Surname.Should().Be(dto.Surname);
        newUser.Email.Should().Be(dto.Email);
        newUser.Username.Should().Be(dto.Username);
        newUser.ContactNumber.Should().Be(dto.ContactNumber);
        newUser.Role.Should().Be(dto.Role);
    }
    
    [Fact(DisplayName = "Researcher is not able to create a new Admin user")]
    public async Task ResearcherDeniedToCreateAdminUser()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Researcher)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new CreateUserDto("name", "surname", "example@gmail.com",
            "username", "Password123", "+393456543333",
            UserRole.Admin);
        

        var response = await httpClient.PostAsJsonAsync(
            $"users",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var dbContext = _fixture.GetService<AppDbContext>();

        var newUser = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto.Email && u.Role == dto.Role);
        
        newUser.Should().BeNull();
    }
    
    [Fact(DisplayName = "Researcher is not able to create a new Researcher user")]
    public async Task ResearcherDeniedToCreateResearcherUser()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Researcher)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto = new CreateUserDto("name", "surname", "example@gmail.com",
            "username", "Password123", "+393456543333",
            UserRole.Researcher);
        

        var response = await httpClient.PostAsJsonAsync(
            $"users",
            dto
        );

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var dbContext = _fixture.GetService<AppDbContext>();

        var newUser = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto.Email && u.Role == dto.Role);
        
        newUser.Should().BeNull();
    }
    
    [Fact(DisplayName = "Is not possible to create more than 1 account with the same email")]
    public async Task SameEmailNotPossible()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto1 = new CreateUserDto("name1", "surname1", "example3@gmail.com",
            "username3", "Password123", "+393456543322",
            UserRole.Researcher);
        

        var response1 = await httpClient.PostAsJsonAsync(
            $"users",
            dto1
        );
        
        var dto2 = new CreateUserDto("name2", "surname2", "example3@gmail.com",
            "username4", "Password123", "+393456543223",
            UserRole.Researcher);
        
        var response2 = await httpClient.PostAsJsonAsync(
            $"users",
            dto2
        );

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var dbContext = _fixture.GetService<AppDbContext>();

        var User1 = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto1.Email && u.Username == dto1.Username);
        
        User1.Should().NotBeNull();
        User1.Name.Should().Be(dto1.Name);
        User1.Surname.Should().Be(dto1.Surname);
        User1.Email.Should().Be(dto1.Email);
        User1.Username.Should().Be(dto1.Username);
        User1.ContactNumber.Should().Be(dto1.ContactNumber);
        User1.Role.Should().Be(dto1.Role);
        
        var User2 = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto2.Email && u.Username == dto2.Username);
        
        User2.Should().BeNull();
    }
    
    [Fact(DisplayName = "Is not possible to create more than 1 account with the same username")]
    public async Task SameUsernameNotPossible()
    {
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();


        // Act


        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dto1 = new CreateUserDto("name1", "surname1", "example5@gmail.com",
            "username5", "Password123", "+3934565435322",
            UserRole.Researcher);
        

        var response1 = await httpClient.PostAsJsonAsync(
            $"users",
            dto1
        );
        
        var dto2 = new CreateUserDto("name2", "surname2", "example6@gmail.com",
            "username5", "Password123", "+3934565435353",
            UserRole.Researcher);
        
        var response2 = await httpClient.PostAsJsonAsync(
            $"users",
            dto2
        );

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var dbContext = _fixture.GetService<AppDbContext>();

        var User1 = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto1.Email && u.Username == dto1.Username);
        
        User1.Should().NotBeNull();
        User1.Name.Should().Be(dto1.Name);
        User1.Surname.Should().Be(dto1.Surname);
        User1.Email.Should().Be(dto1.Email);
        User1.Username.Should().Be(dto1.Username);
        User1.ContactNumber.Should().Be(dto1.ContactNumber);
        User1.Role.Should().Be(dto1.Role);
        
        var User2 = await dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(u => u.Email == dto2.Email && u.Username == dto2.Username);
        
        User2.Should().BeNull();
    }
}