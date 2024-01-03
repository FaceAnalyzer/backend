using System.Net;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.StimuliTests;

public class CreateStimuli : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateStimuli(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "User can successfully create a Stimuli")]
    public async Task UserCreateStimuli()
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
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);
        
        await dbContext.SaveChangesAsync();
        


        //Act
        var dto = new CreateStimuliDto("Fake link", "Name of the stimulus", experiment.Id, "Fake description");

        var response = await httpClient.PostAsJsonAsync(
            $"stimuli",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}