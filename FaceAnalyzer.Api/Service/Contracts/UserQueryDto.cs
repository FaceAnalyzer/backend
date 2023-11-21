using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record class UserQueryDto(int? ProjectId, UserRole? Role);