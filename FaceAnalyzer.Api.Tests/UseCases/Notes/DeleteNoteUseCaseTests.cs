using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Notes;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Tests.UseCases.Notes;

public class DeleteNoteUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete a Note")]
    public async Task Should_Delete_Note_Into_DB()
    {
        var services = new IsolatedUseCaseTestServices<DeleteNoteUseCase>("DeleteNoteUseCaseTests",
            new NoteMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        // Arrange
        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Add(project);


        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Add(experiment);

        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        dbContext.Add(creator);

        var note = new Note
        {
            Description = "Dummy description",
            CreatorId = creator.Id,
            ExperimentId = experiment.Id
        };
        dbContext.Add(note);
        await dbContext.SaveChangesAsync();

        var savedNote = dbContext.Note.Find(note.Id);
        


        var request = new DeleteNoteCommand(note.Id);


        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        savedNote = await dbContext.Note
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync();

        Assert.Equal(savedNote.DeletedAt.HasValue, true);
    }

    [Fact(DisplayName = "Should throw Invalid Argument exception when the note to delete does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingExperiment()
    {
        var services = new IsolatedUseCaseTestServices<DeleteNoteUseCase>("EditNoteUseCaseTests",
            new NoteMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;
        var request = new DeleteNoteCommand(100);

        // Act
        var act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        var errorAssertion = await act.Should()
            .ThrowAsync<InvalidArgumentsException>();

        var exception = new InvalidArgumentsExceptionBuilder()
            .AddArgument(nameof(request.Id),
                $"no note with this id ({request.Id}) was found")
            .Build();

        errorAssertion.And.Message.Should().Be(exception.Message);
    }
}