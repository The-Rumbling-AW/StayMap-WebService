using backendAppsWeb.Posts.Application.QueryService;
using backendAppsWeb.Posts.Domain.Model.Commands;
using backendAppsWeb.Posts.Domain.Model.Queries;
using backendAppsWeb.Posts.Domain.Services;
using backendAppsWeb.Posts.Domain.Services.Command;
using backendAppsWeb.Posts.Domain.Services.Query;
using backendAppsWeb.Posts.Interfaces.Resources;
using backendAppsWeb.Posts.Interfaces.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendAppsWeb.Posts.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostCommandService _postCommandService;
    private readonly IPostQueryService _postQueryService;

    public PostController(
        IPostCommandService postCommandService,
        IPostQueryService postQueryService)
    {
        _postCommandService = postCommandService;
        _postQueryService = postQueryService;
    }

    [HttpPost("communities/{communityId}/posts")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePost(
        [FromRoute] int communityId,
        [FromQuery] int userId,
        [FromBody] CreatePostResource resource)
    {
        var command = new CreatePostCommand(
            communityId,
            resource.Content,
            userId,                 // ✔️ ahora sí, en la posición correcta
            resource.ImageUrl
        );


        var result = await _postCommandService.Handle(command);
        if (result is null)
            return BadRequest("No se pudo crear el comentario.");

        var resourceResult = PostResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(CreatePost), new { id = result.Id }, resourceResult);
    }


    //  GET: /api/v1/comments/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostById(int id)
    {
        var query = new GetPostById(id);
        var comment = await _postQueryService.Handle(query);
        
        if (comment is null)
            return NotFound($"No se encontró un comentario con ID {id}.");

        var resource = PostResourceFromEntityAssembler.ToResourceFromEntity(comment);
        return Ok(resource);
    }
    
    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> DeletePost(int id)
    {
        var command = new DeletePostCommand(id);
        var success = await _postCommandService.Handle(command);
    
        if (!success)
            return NotFound($"No se encontró el comentario con ID {id}.");

        return NoContent();
    }
    
    [HttpGet("by-community/{communityId}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get all posts by community ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(IEnumerable<PostResource>))]
    public async Task<IActionResult> GetPostsByCommunityId([FromRoute] int communityId)
    {
        var query = new GetPostsByCommunityId(communityId);
        var posts = await _postQueryService.Handle(query);

        var response = posts.Select(PostResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(response);
    }

    

}
