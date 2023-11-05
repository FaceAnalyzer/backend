using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class DeleteReactionUseCase : BaseUseCase, IRequestHandler<DeleteReactionCommand>
{
    public DeleteReactionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
    {
        var reaction = await DbContext.FindAsync<Reaction>(request.Id);
        if (reaction is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no reaction with this id ({request.Id}) was found")
                .Build();
        }

        reaction.DeletedAt = DateTime.UtcNow;
        DbContext.Update(reaction);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
    
}