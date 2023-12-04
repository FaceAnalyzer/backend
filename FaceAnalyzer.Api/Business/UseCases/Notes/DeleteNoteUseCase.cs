using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Notes;

public class DeleteNoteUseCase: BaseUseCase, IRequestHandler<DeleteNoteCommand>
{
    public DeleteNoteUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
    
    public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await DbContext.Note
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (note is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no note with this id ({request.Id}) was found")
                .Build();
        }

        DbContext.Delete(note);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

}