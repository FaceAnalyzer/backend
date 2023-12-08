using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.UseCases.Notes;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Tests.UseCases.Notes;

public class DeleteNoteUseCaseTests : UseCaseTestBase<DeleteNoteUseCase>
{
    public DeleteNoteUseCaseTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
    
    [Fact(DisplayName = "Successfully Delete a Note")]
    public async Task Should_Delete_Note_Into_DB()
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
        
        var savedNote = DbContext.Note.Find(note.Id);
        
        Assert.NotNull(savedNote);
        Assert.Equal(note.Description, savedNote.Description);
        Assert.Equal(note.Id, savedNote.Id);
        Assert.Equal(experiment.Id , savedNote.ExperimentId);
        Assert.Equal(creator.Id , savedNote.CreatorId);
        
        
        
        var request = new DeleteNoteCommand(note.Id);
        

        // Act
        await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        savedNote = DbContext.Note
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        //Assert.Equal(savedNote.DeletedAt.HasValue, true);
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when the note to delete does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingExperiment()
    {
        var request = new DeleteNoteCommand(100);
        
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
    
}