using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Notes;

public class EditNoteUseCase : BaseUseCase, IRequestHandler<EditNoteCommand, NoteDto>
{
    public EditNoteUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<NoteDto> Handle(EditNoteCommand request, CancellationToken cancellationToken)
    {
        var note = DbContext.Find<Note>(request.Id);
        if (note is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no note with this id ({request.Id}) was found")
                .Build();
        }

        if (request.CreatorId is not null)
        {
            var creator = DbContext.Find<User>(request.CreatorId);
            if (creator is null)
            {
                throw new InvalidArgumentsExceptionBuilder()
                    .AddArgument(nameof(request.CreatorId),
                        $"no user with this id ({request.CreatorId}) was found")
                    .Build();
            }

            note.CreatorId = creator.Id;
        }

        if (request.ExperimentId is not null)
        {
            var experiment = DbContext.Find<Experiment>(request.ExperimentId);
            if (experiment is null)
            {
                throw new InvalidArgumentsExceptionBuilder()
                    .AddArgument(nameof(request.ExperimentId),
                        $"no experiment with this id ({request.ExperimentId}) was found")
                    .Build();
            }

            note.ExperimentId = experiment.Id;
        }

        note.Description = request.Description;
        note.UpdatedAt = DateTime.UtcNow;
        DbContext.Update(note);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<NoteDto>(note);
    }
}