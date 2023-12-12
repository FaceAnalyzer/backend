using System.Net;
using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("users")]
// [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
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
    [SwaggerOperation("Retrieve a list of users.",
        "Retrieve a list of users. The list can be filtered by projects [projectId] or roles [userRole].",
        OperationId = $"{nameof(Data.Entities.User)}_get_list")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(QueryResult<UserDto>))]
    public async Task<ActionResult<QueryResult<UserDto>>> Get([FromQuery] UserQueryDto dto)
    {
        var query = _mapper.Map<GetUsersQuery>(dto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [SwaggerOperation("Retrieve a single user.",
        "Retrieve a single user given its Id.",
        OperationId = $"{nameof(Data.Entities.User)}_get")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
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
    [SwaggerOperation("Modify a user.",
        "Modify a single user given its Id. [name], [surname], [email], [username], [contactNumber], and [role] all can be modified",
        OperationId = $"{nameof(Data.Entities.User)}_edit")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [SwaggerRequestExample(typeof(EditUserDto), typeof(EditUserDtoExample))]
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
    [SwaggerOperation("Create a user.",
        "Create a specifying its [name], [surname], [email], [username], [password],  [contatNumber], and [role].",
        OperationId = $"{nameof(Data.Entities.User)}_create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(UserDto))]
    [SwaggerRequestExample(typeof(CreateUserDto), typeof(CreateUserDtoExample))]
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
    [SwaggerOperation("Delete a user.",
        "Delete a single user given its Id.",
        OperationId = $"{nameof(Data.Entities.User)}_delete")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}