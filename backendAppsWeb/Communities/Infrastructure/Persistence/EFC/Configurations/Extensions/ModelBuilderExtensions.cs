using System.Text.Json;
using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Model.Entity;
using backendAppsWeb.Communities.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backendAppsWeb.Communities.Infrastructure.Persistence.EFC.Configurations.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCommunityConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>().HasKey(x => x.Id);
        modelBuilder.Entity<Community>().Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        modelBuilder.Entity<Community>().Property(x => x.Name).HasMaxLength(100).IsRequired();
       // modelBuilder.Entity<Community>().Property(x => x.MemberQuantity).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Community>().Property(x => x.Description).HasMaxLength(500);
        
    
        
        var tagsConverter = new ValueConverter<Tags, string>(
            v => JsonSerializer.Serialize(v.Value, new JsonSerializerOptions()),
            v => new Tags(JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())!)
        );

        var membersConverter = new ValueConverter<Members, string>(
            v => JsonSerializer.Serialize(v.UserIds, new JsonSerializerOptions()),
            v => new Members(JsonSerializer.Deserialize<List<int>>(v, new JsonSerializerOptions())!)
        );
        

    //    modelBuilder.Entity<Community>().Property(x => x.Tags)
      //      .HasConversion(tagsConverter)
       //     .HasColumnType("TEXT");

        //modelBuilder.Entity<Community>().Property(x => x.Members)
        //    .HasConversion(membersConverter)
        //    .HasColumnType("TEXT");
        
        modelBuilder.Entity<Community>()
            .HasMany(c => c.Posts)             // 👈 Usa propiedad de navegación
            .WithOne(c => c.Community)            // 👈 Usa propiedad de navegación inversa
            .HasForeignKey(c => c.CommunityId)    // 👈 Usa el nombre correcto
            .OnDelete(DeleteBehavior.Cascade);
      
        
        // Tabla intermedia User ↔ Community
        modelBuilder.Entity<CommunityMember>()
            .ToTable("communitymembers")
            .HasKey(cm => new { cm.CommunityId, cm.UserId });

        modelBuilder.Entity<CommunityMember>()
            .HasOne(cm => cm.Community)
            .WithMany(c => c.UserLinks)
            .HasForeignKey(cm => cm.CommunityId);

        modelBuilder.Entity<CommunityMember>()
            .HasOne(cm => cm.User)
            .WithMany(u => u.CommunityLinks)
            .HasForeignKey(cm => cm.UserId);

        
    }
}