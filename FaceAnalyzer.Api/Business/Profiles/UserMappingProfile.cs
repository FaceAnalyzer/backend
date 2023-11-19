using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;


namespace FaceAnalyzer.Api.Business.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, DeleteUserCommand>().ReverseMap();
        CreateMap<User, EditUserCommand>().ReverseMap();
    }
    
}