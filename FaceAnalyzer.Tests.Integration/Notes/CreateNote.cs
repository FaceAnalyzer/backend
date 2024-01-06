using System.Net;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using System.Net.Http.Json;
using FluentAssertions;

namespace FaceAnalyzer.Tests.Integration.Notes;

public class CreateNote :  IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public CreateNote(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "User can successfully create a Note")]
    public async Task UserCreateNote()
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
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        dbContext.Users.Add(creator);
        
        await dbContext.SaveChangesAsync();
        


        //Act
        var dto = new CreateNoteDto("Example Description Note", experiment.Id, creator.Id);

        var response = await httpClient.PostAsJsonAsync(
            $"notes",
            dto
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}