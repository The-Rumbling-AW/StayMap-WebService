using backendAppsWeb.Communities.Application.CommandServices;
using backendAppsWeb.Communities.Application.QueryServices;
using backendAppsWeb.Communities.Domain.Repositories;
using backendAppsWeb.Communities.Domain.Services;
using backendAppsWeb.Communities.Domain.Services.Command;
using backendAppsWeb.Communities.Domain.Services.Query;
using backendAppsWeb.Communities.Infrastructure.Persistence.EFC.Repositories;
using backendAppsWeb.Concerts.Application.CommandServices;
using backendAppsWeb.Concerts.Application.QueryService;
using backendAppsWeb.Concerts.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using backendAppsWeb.Concerts.Domain.Services;
using backendAppsWeb.Concerts.Domain.Services.Command;
using backendAppsWeb.Concerts.Domain.Services.Query;
using backendAppsWeb.Concerts.Infrastructure.Persistence.EFC.Repositories;
using backendAppsWeb.IAM.Application.Internal.CommandServices;
using backendAppsWeb.IAM.Application.Internal.OutboundServices;
using backendAppsWeb.IAM.Application.Internal.QueryServices;
using backendAppsWeb.IAM.Domain.Repositories;
using backendAppsWeb.IAM.Domain.Services;
using backendAppsWeb.IAM.Infrastructure.Hashing.BCrypt.Services;
using backendAppsWeb.IAM.Infrastructure.Persistence.EFC.Repositories;
using backendAppsWeb.IAM.Infrastructure.Pipeline.Middleware.Components;
using backendAppsWeb.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using backendAppsWeb.IAM.Infrastructure.Tokens.JWT.Configuration;
using backendAppsWeb.IAM.Infrastructure.Tokens.JWT.Services;
using backendAppsWeb.IAM.Interfaces.ACL;
using backendAppsWeb.IAM.Interfaces.ACL.Services;
using backendAppsWeb.Posts.Application.QueryService;
using backendAppsWeb.Posts.Domain.Repositories;
using backendAppsWeb.Posts.Domain.Services.Command;
using backendAppsWeb.Posts.Domain.Services.Query;
using backendAppsWeb.Posts.Infrastructure.Persistence.EFC.Repositories;
//using backendAppsWeb.Profile.Application.CommandServices;
//using backendAppsWeb.Profile.Domain.Repositories;
//using backendAppsWeb.Profile.Domain.Services;
//using backendAppsWeb.Profile.Infrastructure.Persistence.EFC.Repositories;
using backendAppsWeb.Shared.Domain.Repositories;
using backendAppsWeb.Shared.Infrastructure.Interfaces.ASP.Configuration;
using backendAppsWeb.Shared.Infrastructure.Mediator.Cortex.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;
using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CatchUp Platform API",
        Version = "v1",
        Description = "API for managing favorite sources in CatchUp Platform"
    });
    options.EnableAnnotations();
    
    options.CustomSchemaIds(type => type.FullName);
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    
});

// Configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new KebabCaseRouteNamingConvention());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // ⬅️ Esto convierte enums a string
    });
// Add Database Connection  
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Verify Database Connection String
if (connectionString is null)
    throw new Exception("Database Connection String is null");

// Configure Database Context and Logging levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableDetailedErrors();
    });

// Configure Dependency Injection for Repositories

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// News Bounded Context Injection Configuration
builder.Services.AddScoped<IConcertRepository, ConcertRepository>();
builder.Services.AddScoped<IConcertCommandService, ConcertCommandService>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICommunityCommandService, CommunityCommandService>();
builder.Services.AddScoped<ICommunityQueryService, CommunityQueryService>();
builder.Services.AddScoped<IPostQueryService, PostQueryService>();

//builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
//builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();

//IAM

builder.Services.Configure<TokenSettings>(
    builder.Configuration.GetSection("TokenSettings")
);



// 🔐 JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["TokenSettings:Secret"] 
                    ?? throw new InvalidOperationException("Missing JWT secret")
                )
            )
        };

        // 👇 Eventos para depuración
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ Token inválido: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ Token válido para: " + context.Principal.Identity?.Name);
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();


//builder.Services.AddScoped<IPurchaseOrderQueryService, PurchaseOrderQueryService>();
// Comments Bounded Context Injection Configuration
builder.Services.AddScoped<IConcertQueryService, ConcertQueryService>();
builder.Services.AddScoped<IPostsRepository, PostRepository>();
builder.Services.AddScoped<IPostCommandService, PostCommandService>();


// Add the Cortex.Mediator package to your project
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));


var app = builder.Build();

using( var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;   
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.


    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CatchUp Platform API v1");
        options.RoutePrefix = "swagger"; // Así accedes en /swagger
    });


app.UseRouting();

app.UseCors("AllowAllPolicy");

app.UseAuthentication();       
app.UseAuthorization();        

app.UseRequestAuthorization(); 

app.MapControllers();  ;         

app.Run();
