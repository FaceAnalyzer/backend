using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Profiles;

public class StimuliMappingProfile : Profile
{
    public StimuliMappingProfile()
    {
        CreateMap<Stimuli, StimuliDto>()
            .ReverseMap();
    }
}