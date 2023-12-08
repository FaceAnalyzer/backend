using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.UseCases.Notes;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Tests.UseCases.Notes;

public class EditNoteUseCaseTests : UseCaseTestBase<EditNoteUseCase>
{
    [Fact(DisplayName = "Successfully Edited a Note")]
    public async Task Should_Edit_Note_Into_DB()
    {
        // Arrange
        CleanDatabase();
        var project = new Project
        {
            Name = "Dummy Project"
        };
        DbContext.Add(project);
        
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        DbContext.Add(experiment);
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        DbContext.Add(creator);
        
        var note = new Note
        {
            Description = "Dummy description",
            CreatorId = creator.Id,
            ExperimentId = experiment.Id
        };
        DbContext.Add(note);
        
        DbContext.SaveChanges();
        
        
        var description = "Description changed.";
        var request = new EditNoteCommand(note.Id, description, experiment.Id, creator.Id);

        // Act
        
        var result = await UseCase.Handle(request, CancellationToken.None);

        // Assert
        var savedNote = DbContext.Note
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        
        Assert.NotNull(savedNote);
        //Assert.Equal(description, savedNote.Description);
        Assert.Equal(note.Id, savedNote.Id);
        Assert.Equal(experiment.Id , savedNote.ExperimentId);
        Assert.Equal(creator.Id , savedNote.CreatorId);
        
        
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when experiment does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingExperiment()
    {

        
        CleanDatabase();
        var project = new Project
        {
            Name = "Dummy Project"
        };
        DbContext.Add(project);
        
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        DbContext.Add(experiment);
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        DbContext.Add(creator);
        
        var note = new Note
        {
            Description = "Dummy description",
            CreatorId = creator.Id,
            ExperimentId = experiment.Id
        };
        DbContext.Add(note);
        DbContext.SaveChanges();
        
        var description = "Description changed.";
        var request = new EditNoteCommand(note.Id, description, 128, creator.Id);

        // Act
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        var errorAssertion  = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();
        
        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.ExperimentId),
                $"no experiment with this id ({request.ExperimentId}) was found")
            .Build();
        
        errorAssertion.And.Message.Should().Be(exception.Message);
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when creator does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingCreator()
    {

        
        CleanDatabase();
        var project = new Project
        {
            Name = "Dummy Project"
        };
        DbContext.Add(project);
        
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        DbContext.Add(experiment);
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        DbContext.Add(creator);
        
        var note = new Note
        {
            Description = "Dummy description",
            CreatorId = creator.Id,
            ExperimentId = experiment.Id
        };
        DbContext.Add(note);
        DbContext.SaveChanges();
        
        var description = "Description changed.";
        var request = new EditNoteCommand(note.Id, description, experiment.Id, 128);

        // Act
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        var errorAssertion  = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();
        
        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.CreatorId),
                $"no user with this id ({request.CreatorId}) was found")
            .Build();
        
        errorAssertion.And.Message.Should().Be(exception.Message);
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when note does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingNote()
    {

        
        CleanDatabase();
        var project = new Project
        {
            Name = "Dummy Project"
        };
        DbContext.Add(project);
        
        
        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        DbContext.Add(experiment);
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        DbContext.Add(creator);
        
        DbContext.SaveChanges();
        
        var description = "Description changed.";
        var request = new EditNoteCommand(10, description, experiment.Id, creator.Id);

        // Act
        var act = async () => await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        var errorAssertion  = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();
        
        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.Id),
                $"no note with this id ({request.Id}) was found")
            .Build();
        
        errorAssertion.And.Message.Should().Be(exception.Message);
    }

    public EditNoteUseCaseTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
    
}