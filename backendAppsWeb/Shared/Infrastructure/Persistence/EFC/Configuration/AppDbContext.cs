
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

using backendAppsWeb.Communities.Infrastructure.Persistence.EFC.Configurations.Extensions;
using backendAppsWeb.Concerts.Infrastructure.Persistence.EFC.Configurations.Extensions;
using backendAppsWeb.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using backendAppsWeb.Posts.Infrastructure.Persistence.EFC.Configurations.Extensions;
//using backendAppsWeb.Profile.Infrastructure.Persistence.EFC.Configurations.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConcertConfiguration();
        modelBuilder.ApplyCommunityConfiguration();
        modelBuilder.ApplyPostConfiguration();
       // modelBuilder.ApplyProfileConfiguration();
       
        modelBuilder.ApplyIAMConfiguration();
        modelBuilder.UseSnakeCaseNamingConvention();
        
    }
}