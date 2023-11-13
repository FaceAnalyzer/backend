using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Emotions;

public class CreateEmotionUseCase: BaseUseCase, IRequestHandler<CreateEmotionCommand, EmotionDto>
{
    
    public CreateEmotionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
    
    public async Task<EmotionDto> Handle(CreateEmotionCommand request, CancellationToken cancellationToken)
    {
        var reaction = DbContext.Find<Reaction>(request.ReactionId);
        if (reaction is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.ReactionId),
                    $"no reaction with this id ({request.ReactionId}) was found")
                .Build();
        }

        var emotion = Mapper.Map<Emotion>(request);
        DbContext.Add(emotion);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<EmotionDto>(emotion);
    }
}