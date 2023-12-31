﻿using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;


namespace FaceAnalyzer.Api.Business.UseCases.Notes;

public class CreateNoteUseCase : BaseUseCase, IRequestHandler<CreateNoteCommand, NoteDto>
{
    public CreateNoteUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
    
    public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var user = DbContext.Find<User>(request.CreatorId);
        if (user is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.CreatorId),
                    $"no user with this id ({request.CreatorId}) was found")
                .Build();
        }
            
        var experiment = DbContext.Find<Experiment>(request.ExperimentId);
        if (experiment is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.ExperimentId),
                    $"no experiment with this id ({request.ExperimentId}) was found")
                .Build();
        }
        

        var note = Mapper.Map<Note>(request);
        
        DbContext.Add(note);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<NoteDto>(note);
    }
    
}