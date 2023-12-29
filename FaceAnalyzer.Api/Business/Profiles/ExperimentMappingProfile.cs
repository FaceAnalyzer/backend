using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Profiles;

public class ExperimentMappingProfile: Profile
{
    public ExperimentMappingProfile()
    {
        CreateMap<Experiment, ExperimentDto>().ReverseMap();
        CreateMap<Experiment, CreateExperimentCommand>().ReverseMap();
        CreateMap<Experiment, EditExperimentCommand>().ReverseMap();
        CreateMap<Experiment, DeleteExperimentCommand>().ReverseMap();
        CreateMap<Experiment, ExportExperimentDto>()
            .ReverseMap();
    }
}