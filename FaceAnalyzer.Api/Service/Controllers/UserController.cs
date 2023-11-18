using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<List<UserDto>>> Get()
    {
        var result = await _mediator.Send(new GetUsersQuery(null));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetUsersQuery(id));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException("User", id);
        }

        return Ok(result.Items.FirstOrDefault());
    }
    
    [HttpPut("{id}")]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Edit(int id, [FromBody] EditUserDto dto)
    {
        var command = new EditUserCommand(
            id,
            dto.Name,
            dto.Surname,
            dto.Email,
            dto.Username,
            dto.ContactNumber,
            dto.Role);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        var command = new CreateUserCommand(
            Name: dto.Name,
            Surname: dto.Surname,
            Email: dto.Email,
            Username: dto.Username,
            Password: dto.Password,
            ContactNumber: dto.ContactNumber,
            Role: dto.Role
        );
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }
    
    [HttpDelete("{id}")]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
    
    
}