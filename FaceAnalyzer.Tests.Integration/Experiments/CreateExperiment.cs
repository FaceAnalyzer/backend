using System.Net;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;


namespace FaceAnalyzer.Tests.Integration.Experiments;

public class CreateExperiment :  IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateExperiment(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "User can successfully create an Experiment")]
    public async Task UserCreateExperiment()
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
        


        //Act
        var dto = new CreateExperimentDto("ExampleName", "ExampleDescription", project.Id);

        var response = await httpClient.PostAsJsonAsync(
            $"experiments",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}