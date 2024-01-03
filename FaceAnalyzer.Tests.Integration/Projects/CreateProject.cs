using System.Net;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Projects;

public class CreateProject : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateProject(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "Admin can successfully create a Project")]
    public async Task AdminCreateProject()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        


        //Act
        var dto = new CreateProjectDto("Example Project Name");

        var response = await httpClient.PostAsJsonAsync(
            $"projects",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact(DisplayName = "Researcher cannot create a Project")]
    public async Task ResearcherCannotCreateProject()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Researcher)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        


        //Act
        var dto = new CreateProjectDto("Example Project Name");

        var response = await httpClient.PostAsJsonAsync(
            $"projects",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
}