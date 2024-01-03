using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;


namespace FaceAnalyzer.Tests.Integration.Notes;

public class GetNotes
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public GetNotes(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "User can successfully get all notes")]
    public async Task UserGetAllNotes()
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

        // Act
        var getResponse = await httpClient.GetAsync($"notes");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"notes");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<NoteDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full notes list from the database.
        var dbNotes = dbContext.Note.ToList();
        foreach (var note2 in response!.Items)
        {
            var dbNote = dbNotes.Find(n =>
                n.Id == note2.Id);
            dbNote.Description.Should().Be(note2.Description);
            dbNote.ExperimentId.Should().Be(note2.ExperimentId);
            dbNote.CreatorId.Should().Be(note2.CreatorId);
        }
    }
    
    [Fact(DisplayName = "User can successfully get all notes given an experiment")]
    public async Task UserGetAllNotesByExperimentId()
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

        // Act
        var getResponse = await httpClient.GetAsync($"notes?ExperimentId={experiment.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"notes?ExperimentId={experiment.Id}");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = JsonSerializer.Deserialize<QueryResult<NoteDto>>(jsonResponse, jsonOptions);

        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Count.Should().Be(response.Count);

        // Check that the returned list is the full notes list from the database.
        var dbNotes = dbContext.Note.ToList();
        foreach (var note2 in response!.Items)
        {
            var dbNote = dbNotes.Find(n =>
                n.Id == note2.Id);
            dbNote.Description.Should().Be(note2.Description);
            dbNote.ExperimentId.Should().Be(note2.ExperimentId);
            dbNote.CreatorId.Should().Be(note2.CreatorId);
        }
    }
    
    [Fact(DisplayName = "User can successfully get a note using the Id")]
    public async Task UserGetNoteById()
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

        // Act
        var getResponse = await httpClient.GetAsync($"notes/{note.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonResponse = await httpClient.GetStringAsync($"notes/{note.Id}");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };
        
        var response = JsonSerializer.Deserialize<Note>(jsonResponse, jsonOptions);
        
        // Assert
        response.Should().NotBeNull();
        response?.ExperimentId.Should().Be(note.ExperimentId);
        response?.CreatorId.Should().Be(note.CreatorId);
        response?.Description.Should().Be(note.Description);
    }

    
    [Fact(DisplayName = "Getting a non-existing note returns 404")]
    public async Task GetNonExistingNoteReturns404()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to get a note that does not exist
        var getResponse = await httpClient.GetAsync($"notes/{9999}"); // Assuming 9999 is a non-existing note Id

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}