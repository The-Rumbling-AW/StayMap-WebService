using System.Text.RegularExpressions;
using backendAppsWeb.IAM.Application.Internal.OutboundServices;
using backendAppsWeb.IAM.Domain.Model.Queries;
using backendAppsWeb.IAM.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace backendAppsWeb.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        Console.WriteLine("Entering InvokeAsync");

        var endpoint = context.GetEndpoint();

        // ✅ Protección contra null y rutas sin endpoint (como Swagger)
        if (endpoint == null || endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            Console.WriteLine("Skipping authorization (null endpoint or AllowAnonymous)");
            await next(context);
            return;
        }

        Console.WriteLine("Entering authorization");

        // ✅ Obtener token del header Authorization
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.StartsWith("Bearer ") == true
            ? authHeader["Bearer ".Length..].Trim()
            : null;

        // ❌ Token faltante
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Null or invalid token");

        // ✅ Validar token
        var userId = await tokenService.ValidateToken(token);
        if (userId == null)
            throw new Exception("Invalid token");

        // ✅ Obtener usuario
        var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
        var user = await userQueryService.Handle(getUserByIdQuery);
        context.Items["User"] = user;

        Console.WriteLine("Successful authorization. Continuing...");
        await next(context);
    }
}