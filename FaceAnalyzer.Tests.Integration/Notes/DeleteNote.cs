using System.Net;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Tests.Integration.Notes;

public class DeleteNote : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;


    public DeleteNote(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "User can successfully delete a note")]
    public async Task UserDeleteNote()
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

        var note = new Note
        {
            ExperimentId = experiment.Id,
            CreatorId = creator.Id,
            Description = "Fake Description"
        };
        dbContext.Note.Add(note);
        await dbContext.SaveChangesAsync();

        note.Should().NotBeNull();

        // Act
        // Delete the note
        var deleteResponse = await httpClient.DeleteAsync($"notes/{note.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Check that the note has been deleted
        var deletedNote = await dbContext.Note.FirstOrDefaultAsync(u => u.Id == note.Id);
        deletedNote.DeletedAt.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Trying to delete a non existing note return bad request")]
    public async Task NotPossibleToDeleteNonExistingNote()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Delete the note
        var deleteResponse = await httpClient.DeleteAsync($"notes/{999}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}