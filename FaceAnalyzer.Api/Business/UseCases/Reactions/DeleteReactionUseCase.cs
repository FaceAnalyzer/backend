using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;



namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class DeleteReactionUseCase : BaseUseCase, IRequestHandler<DeleteReactionCommand, ReactionDto>
{
    public DeleteReactionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task<ReactionDto> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
    {
        var reaction = DbContext.Find<Reaction>(request.Id);
        if (reaction is null)
        {
            throw new Exception();
        }

        reaction.DeletedAt = DateTime.UtcNow;
        DbContext.Update(reaction);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ReactionDto>(reaction);
    }
    
}