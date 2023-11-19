using System.Net;
using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UserController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<QueryResult<UserDto>>> Get([FromQuery] UserQueryDto dto)
    {
        var query = _mapper.Map<GetUsersQuery>(dto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetUsersQuery
        {
            Id = id
        });
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException("User", id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<UserDto>> Edit(int id, [FromBody] EditUserDto dto)
    {
        var command = _mapper.Map<EditUserCommand>(dto);
        command = command with { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        var command = _mapper.Map<CreateUserCommand>(dto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<NoContentResult> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}