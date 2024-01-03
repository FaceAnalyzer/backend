using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Tests.Integration.Projects;

public class DeleteProject : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public DeleteProject(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "Admin can successfully delete a project")]
    public async Task AdminDeleteProject()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        project.Should().NotBeNull();

        // Act
        // Delete the project
        var deleteResponse = await httpClient.DeleteAsync($"projects/{project.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Check that the project has been deleted
        var deletedProject = await dbContext.Projects.FirstOrDefaultAsync(u => u.Id == project.Id);
        deletedProject.DeletedAt.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Researcher cannot delete a project")]
    public async Task ResearcherCannotDeleteProject()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Researcher)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        project.Should().NotBeNull();

        // Act
        // Delete the project
        var deleteResponse = await httpClient.DeleteAsync($"projects/{project.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        
        // Check that the project has not been deleted
        var deletedProject = await dbContext.Projects.FirstOrDefaultAsync(u => u.Id == project.Id);
        deletedProject.DeletedAt.Should().BeNull();
    }
    
    [Fact(DisplayName = "Trying to delete a non existing project return bad request")]
    public async Task NotPossibleToDeleteNonExistingProject()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Delete the project
        var deleteResponse = await httpClient.DeleteAsync($"projects/{999}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}