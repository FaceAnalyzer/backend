using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.UseCases.Notes;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Tests.UseCases.Notes;

public class CreateNoteUseCaseTests : UseCaseTestBase<CreateNoteUseCase>
{
    [Fact(DisplayName = "Successfully Store new Note")]
    public async Task Should_Store_Note_Into_DB()
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
        
        DbContext.SaveChanges();
        
        var description = "This is a test note.";
        var request = new CreateNoteCommand(description, experiment.Id, creator.Id);

        // Act
        var savedNote = DbContext.Note
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        Assert.Null(savedNote);
        
        var result = await UseCase.Handle(request, CancellationToken.None);
        
        // Assert
        savedNote = DbContext.Note
            .IgnoreQueryFilters()
            .FirstOrDefault();
        
        Assert.NotNull(savedNote);
        //Assert.Equal(savedNote.Description, description);
        Assert.Equal(savedNote.ExperimentId, experiment.Id);
        Assert.Equal(savedNote.CreatorId, creator.Id);
        
    }
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when experiment does not exists")]
    public async Task? Should_Fail_Storing_Note_NotExistingExperiment()
    {

        
        CleanDatabase();
        
        var creator = new User
        {
            Name = "Dummy CreatorNote user",
        };
        DbContext.Add(creator);
        
        DbContext.SaveChanges();
        
        var description = "This is a test note.";
        var request = new CreateNoteCommand(description, 128, creator.Id);

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
    
    [Fact(DisplayName = "Should throw Invalid Argument exception when user creator does not exists")]
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
        
        
        
        DbContext.SaveChanges();
        
        var description = "This is a test note.";
        var request = new CreateNoteCommand(description, experiment.Id, 128);

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

    public CreateNoteUseCaseTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
    {
    }
    
}