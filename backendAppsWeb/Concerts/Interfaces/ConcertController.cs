using System.Net.Mime;
using backendAppsWeb.Concerts.Domain.Model.Commands;
using backendAppsWeb.Concerts.Domain.Model.Queries;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;
using backendAppsWeb.Concerts.Domain.Services;
using backendAppsWeb.Concerts.Domain.Services.Command;
using backendAppsWeb.Concerts.Domain.Services.Query;
using backendAppsWeb.Concerts.Interfaces.Resources;
using backendAppsWeb.Concerts.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
namespace backendAppsWeb.Concerts.Interfaces;

[ApiController]

[Route("api/v1/concerts")]

[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Concert Management")]
public class ConcertController(
    IConcertCommandService commandService,
    IConcertQueryService queryService
) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create a new concert",
        Description = "Creates a new concert with name, genre, date, status, description and image",
        OperationId = "CreateConcert")]
    [SwaggerResponse(201, "Created", typeof(ConcertResource))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    public async Task<IActionResult> CreateConcert([FromBody] CreateConcertResource resource)
    {
        try
        {
            var command = CreateConcertCommandFromResourceAssembler.toCommandFromResource(resource);
            var created = await commandService.Handle(command);

            if (created == null)
                return BadRequest("Could not create concert.");

            var response = ConcertResourceFromEntityAssembler.toResourceFromEntity(created);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get concert by ID",
        Description = "Retrieves a concert by its ID",
        OperationId = "GetConcertById")]
    [SwaggerResponse(200, "Success", typeof(ConcertResource))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> GetConcertById([FromRoute] int id)
    {
        var query = new GetConcertById(id);
        var result = await queryService.Handle(query);
        if (result == null) return NotFound("Concert not found");
        var response = ConcertResourceFromEntityAssembler.toResourceFromEntity(result);
        return Ok(response);
    }

    [HttpGet("by-date")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get concerts by date",
        Description = "Retrieves all concerts on a specific date",
        OperationId = "GetConcertByDate")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ConcertResource>))]
    public async Task<IActionResult> GetConcertsByDate([FromQuery] DateTime date)
    {
        var query = new GetConcertByDate(date);
        var results = await queryService.Handle(query);
        var response = results.Select(ConcertResourceFromEntityAssembler.toResourceFromEntity);
        return Ok(response);
    }

    [HttpGet("by-name")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get concerts by name",
        Description = "Retrieves all concerts with a given name",
        OperationId = "GetConcertByName")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ConcertResource>))]
    public async Task<IActionResult> GetConcertsByName([FromQuery] string name)
    {
        var query = new GetConcertByName(name);
        var results = await queryService.Handle(query);
        var response = results.Select(ConcertResourceFromEntityAssembler.toResourceFromEntity);
        return Ok(response);
    }

    [HttpGet("list")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get all concerts (public)",
        Description = "Returns a list of all concerts (no token required once middleware is adapted)",
        OperationId = "GetAllConcertsPublic")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ConcertResource>))]
    public async Task<IActionResult> GetConcertList()
    {
        var concerts = await queryService.Handle(new GetAllConcerts());
        var resources = concerts.Select(ConcertResourceFromEntityAssembler.toResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpPost("{concertId}/attend")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Confirm attendance to concert",
        Description = "Adds the user to the concert attendee list",
        OperationId = "AttendConcert")]
    [SwaggerResponse(200, "Attendance confirmed")]
    [SwaggerResponse(400, "Could not confirm")]
    public async Task<IActionResult> AttendConcert(
        [FromRoute] int concertId,
        [FromQuery] int userId)
    {
        var success = await commandService.Handle(new AttendConcertCommand(concertId, userId));

        if (!success)
            return BadRequest("Could not confirm attendance or already confirmed.");

        return Ok("Attendance confirmed");
    }
    
    [HttpPost("{concertId}/toggle-attendance")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Toggle attendance to a concert",
        Description = "Adds or removes the user from the concert attendee list",
        OperationId = "ToggleAttendance")]
    [SwaggerResponse(200, "Attendance toggled")]
    [SwaggerResponse(400, "Could not toggle")]
    public async Task<IActionResult> ToggleAttendance(
        [FromRoute] int concertId,
        [FromQuery] int userId)
    {
        var result = await commandService.ToggleAttendance(concertId, userId);

        if (!result)
            return BadRequest("Could not toggle attendance");

        return Ok("Attendance toggled");
    }
    
    [HttpDelete("{concertId}/attend")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Cancel attendance to a concert",
        Description = "Removes the user from the concert attendee list",
        OperationId = "CancelAttendance")]
    [SwaggerResponse(200, "Attendance canceled")]
    [SwaggerResponse(400, "Could not cancel")]
    public async Task<IActionResult> CancelAttendance([FromRoute] int concertId, [FromQuery] int userId)
    {
        var success = await commandService.CancelAttendance(concertId, userId);

        if (!success)
            return BadRequest("Could not cancel attendance");

        return Ok("Attendance canceled");
    }
    

    [HttpDelete("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Delete concert by ID",
        Description = "Deletes a concert if it exists",
        OperationId = "DeleteConcert")]
    [SwaggerResponse(204, "No Content")]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> DeleteConcert(int id)
    {
        var command = new DeleteConcertCommand(id);
        var result = await commandService.Handle(command);
        if (!result) return NotFound("Concierto no encontrado.");
        return NoContent();
    }
    
    

    [HttpPut("{id}/status")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Update concert status")]
    public async Task<IActionResult> UpdateConcertStatus(int id, [FromQuery] Status status)
    {
        var command = new UpdateConcertStatusCommand(id, status);
        var success = await commandService.Handle(command);

        if (!success) return NotFound("Concert not found");
        return Ok("Concert status updated");
    }

    
    [HttpGet("by-platform/{platform}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get concerts by ticket platform",
        Description = "Returns all concerts that use the specified ticket platform: Teleticket, Ticketmaster, or Joinnus",
        OperationId = "GetConcertsByPlatform")]
    [SwaggerResponse(StatusCodes.Status200OK, "Concerts found", typeof(IEnumerable<ConcertResource>))]
    public async Task<IActionResult> GetConcertsByPlatform(TicketPlatformType platform)
    {
        var concerts = await queryService.GetByPlatformAsync(platform);

        var concertResources = concerts
            .Select(ConcertResourceFromEntityAssembler.toResourceFromEntity)
            .ToList();

        return Ok(concertResources);
    }


    
    
}
