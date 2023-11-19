using System.ComponentModel.DataAnnotations;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Users;

public record EditUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    [EmailAddress] public required string Email { get; init; }
    public required string Username { get; init; }
    [Phone] public string? ContactNumber { get; init; }
    public UserRole Role { get; init; }
}