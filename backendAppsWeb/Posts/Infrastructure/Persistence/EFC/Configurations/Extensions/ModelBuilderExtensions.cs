
using backendAppsWeb.Posts.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace backendAppsWeb.Posts.Infrastructure.Persistence.EFC.Configurations.Extensions;
  public static class ModelBuilderExtensions
    {
        public static void ApplyPostConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(c => c.CommunityId)
                    .IsRequired();

                entity.Property(c => c.UserId) // ✅ nuevo
                    .IsRequired();

                entity.Property(c => c.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(c => c.PostedAt)
                    .IsRequired();

                entity.Property(c => c.ImageUrl)
                    .HasMaxLength(500);

                entity.HasOne(c => c.Community)
                    .WithMany(c => c.Posts)
                    .HasForeignKey(c => c.CommunityId);

                entity.HasOne(c => c.User) // ✅ nuevo
                    .WithMany()            // o .WithMany(u => u.Posts)
                    .HasForeignKey(c => c.UserId);
            });
        }

    }
