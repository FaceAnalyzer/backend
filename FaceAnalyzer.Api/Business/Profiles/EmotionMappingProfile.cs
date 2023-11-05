using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;

namespace FaceAnalyzer.Api.Business.Profiles;

public class EmotionMappingProfile: Profile
{
    public EmotionMappingProfile()
    {
        CreateMap<CreateEmotionDto, CreateEmotionCommand>().ReverseMap();
        CreateMap<CreateEmotionCommand, EmotionDto>().ReverseMap();
        CreateMap<Emotion, CreateEmotionCommand>().ReverseMap();
        CreateMap<Emotion, EmotionDto>().ReverseMap();
    }
}