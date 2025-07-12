using System.Net.Mime;
using backendAppsWeb.IAM.Application.Internal.CommandServices;
using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Domain.Services;
using backendAppsWeb.IAM.Interfaces.REST.Resources;
using backendAppsWeb.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendAppsWeb.IAM.Interfaces.REST;


[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints")]
public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{
    /**
     * <summary>
     *     Sign in endpoint. It allows authenticating a user
     * </summary>
     * <param name="signInResource">The sign-in resource containing username and password.</param>
     * <returns>The authenticated user resource, including a JWT token</returns>
     */
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "Sign in a user",
        OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var authenticatedUser = await userCommandService.Handle(signInCommand);
        var resource =
            AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                authenticatedUser.token);
        return Ok(resource);
    }

    /**
     * <summary>
     *     Sign up endpoint. It allows creating a new user
     * </summary>
     * <param name="signUpResource">The sign-up resource containing username and password.</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign-up",
        Description = "Sign up a new user and return the authenticated profile with token",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was created and authenticated", typeof(AuthenticatedUserResource))]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        await userCommandService.Handle(command);

        var signInCommand = new SignInCommand(signUpResource.Username, signUpResource.Password);
        var (user, token) = await userCommandService.Handle(signInCommand);

        var resource = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(user, token);
        return Ok(resource);
    }
}