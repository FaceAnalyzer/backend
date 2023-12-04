using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Profiles;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<Note, NoteDto>()
            .ReverseMap();
        CreateMap<Note, CreateNoteCommand>().ReverseMap();
        CreateMap<Experiment, EditNoteCommand>().ReverseMap();
        
    }
    
}