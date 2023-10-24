using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Profiles;

public class ExperimentMappingProfile: Profile
{
    public ExperimentMappingProfile()
    {
        CreateMap<Experiment, ExperimentDto>()
            .ReverseMap();
    }
}