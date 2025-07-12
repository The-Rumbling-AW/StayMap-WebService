using System.Net.Mime;
using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Domain.Model.Queries;
using backendAppsWeb.IAM.Domain.Services;
using backendAppsWeb.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using backendAppsWeb.IAM.Interfaces.REST.Resources;
using backendAppsWeb.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendAppsWeb.IAM.Interfaces.REST;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User endpoints")]
public class UsersController(
    IUserQueryService userQueryService,
    IUserCommandService userCommandService) : ControllerBase
{
    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get a user by its id",
        Description = "Get a user by its id",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found", typeof(UserResource))]
    public async Task<IActionResult> GetUserById(int id)
    {
        var getUserByIdQuery = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Get all users",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }

    [HttpPost("{userId}/like")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Like a post", Description = "Adds a post ID to the liked posts list")]
    public async Task<IActionResult> LikePost([FromRoute] int userId, [FromQuery] int postId)
    {
        var command = new LikePostCommand(userId, postId);
        var success = await userCommandService.Handle(command);

        if (!success)
            return BadRequest("No se pudo agregar el like. Verifica si el usuario existe.");

        return Ok("Like registrado.");
    }
    
    
    [HttpPut("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Update user profile")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserResource resource)
    {
        var command = new UpdateUserCommand(id, resource.Name, resource.Email, resource.ProfileImage);
        var success = await userCommandService.Handle(command);

        if (!success)
            return NotFound("User not found");

        return Ok("User updated successfully");
    }

    [HttpDelete("{userId}/like")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Remove like from a post", Description = "Deletes a post ID from the liked posts list")]
    public async Task<IActionResult> RemoveLikePost([FromRoute] int userId, [FromQuery] int postId)
    {
        var command = new DeleteLikePostCommand(userId, postId);
        var success = await userCommandService.Handle(command);

        if (!success)
            return BadRequest("No se pudo eliminar el like. Verifica si el usuario o el post existe.");

        return Ok("Like eliminado correctamente.");
    }
    
    
}