using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Service.Contracts;


namespace FaceAnalyzer.Api.Service.Profiles;

public partial class ServiceMappingProfileRoot : Profile
{
    public ServiceMappingProfileRoot()
    {
        CreateMap<UserQueryDto, GetUsersQuery>();
        CreateMap<EditUserDto, EditUserCommand>();
        CreateMap<CreateUserDto, CreateUserCommand>();
    }
    
}