using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Security;


namespace FaceAnalyzer.Api.Business.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<EditUserCommand, User>();
        CreateMap<CreateUserCommand, User>()
            .ForMember(u => u.Password,
                opt =>
                    opt.Ignore())
            ;
    }
}