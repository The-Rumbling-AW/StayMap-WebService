using System.Net.Mime;
using backendAppsWeb.Communities.Domain.Model.Commands;
using backendAppsWeb.Communities.Domain.Model.Queries;
using backendAppsWeb.Communities.Domain.Services.Command;
using backendAppsWeb.Communities.Domain.Services.Query;
using backendAppsWeb.Communities.Interfaces.Resources;
using backendAppsWeb.Communities.Interfaces.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendAppsWeb.Communities.Interfaces;

[ApiController]
[Route("api/v1/communities")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Community Management")]
public class CommunityController(
    ICommunityCommandService communityService,
    ICommunityQueryService communityQueryService)
: ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create a new Community",
        Description = "Creates a new Community with name, genre, date, status, description and image",
        OperationId = "CreateCommunity")]
    [SwaggerResponse(201, "Created", typeof(CommunityResource))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityResource resource)
    {
        try
        {
            var command = CreateCommunityCommandFromResourceAssembler.toCommandFromResource(resource);
            var created = await communityService.Handle(command);

            if (created == null)
                return BadRequest("Could not create Community.");

            var response = CommunityResourceFromEntityAssembler.toResourceFromEntity(created);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("{communityId}/join")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Join a community", Description = "Adds the user to the community")]
    [SwaggerResponse(200, "Joined successfully")]
    [SwaggerResponse(400, "Could not join")]
    public async Task<IActionResult> JoinCommunity([FromRoute] int communityId, [FromQuery] int userId)
    {
        var success = await communityService.Handle(new JoinCommunityCommand(communityId, userId));

        if (!success)
            return BadRequest("Could not join or already joined.");

        return Ok("Joined successfully");
    }

    
    

    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get a community by ID",
        Description = "Retrieves a community using its ID",
        OperationId = "GetCommunityById")]
    [SwaggerResponse(200, "Success", typeof(CommunityResource))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> GetCommunityById([FromRoute] int id)
    {
        var query = new GetCommunityById(id);
        var result = await communityQueryService.Handle(query);
        if (result == null) return NotFound("Community not found.");
        var response = CommunityResourceFromEntityAssembler.toResourceFromEntity(result);
        return Ok(response);
    }

    [HttpGet("by-name")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get communities by name",
        Description = "Retrieves all communities with the given name",
        OperationId = "GetCommunityByName")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<CommunityResource>))]
    public async Task<IActionResult> GetCommunitiesByName([FromQuery] string name)
    {
        var query = new GetCommunityByName(name);
        var results = await communityQueryService.Handle(query);
        var response = results.Select(CommunityResourceFromEntityAssembler.toResourceFromEntity);
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Delete a community by ID",
        Description = "Deletes a community using its ID",
        OperationId = "DeleteCommunity")]
    [SwaggerResponse(204, "No Content")]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> DeleteCommunity(int id)
    {
        var command = new DeleteCommunityCommand(id);
        var result = await communityService.Handle(command);
        if (!result) return NotFound("Comunidad no encontrada.");
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get all communities")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCommunitiesQuery();
        var result = await communityQueryService
            .Handle(query);
        var resources = result.Select(CommunityResourceFromEntityAssembler.toResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpDelete("{communityId}/join")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Leave a community", Description = "Removes the user from the community")]
    [SwaggerResponse(200, "Left successfully")]
    [SwaggerResponse(400, "Could not leave")]
    public async Task<IActionResult> LeaveCommunity([FromRoute] int communityId, [FromQuery] int userId)
    {
        var success = await communityService.Handle(new LeaveCommunityCommand(communityId, userId));
        if (!success)
            return BadRequest("Could not leave or was not a member.");

        return Ok("Left community successfully");
    }

}
