using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Profiles;

public class ReactionMappingProfile : Profile
{
    public ReactionMappingProfile()
    {
        CreateMap<Reaction, ReactionDto>()
            .ReverseMap();
    }
}