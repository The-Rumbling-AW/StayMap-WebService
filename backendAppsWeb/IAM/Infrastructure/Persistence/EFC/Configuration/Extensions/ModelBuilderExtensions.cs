using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backendAppsWeb.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;


public static class ModelBuilderExtensions
{
    public static void ApplyIAMConfiguration(this ModelBuilder builder)
    {
        var intListToStringConverter = new ValueConverter<List<int>, string>(
            v => string.Join(",", v),
            v => !string.IsNullOrEmpty(v)
                ? v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                : new List<int>());
        
        // ValueComparer requerido para listas
        var intListComparer = new ValueComparer<List<int>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v)),
            c => c.ToList()
        );


        // Basic User fields
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Name).IsRequired();
        builder.Entity<User>().Property(u => u.ProfileImage).IsRequired();
        builder.Entity<User>().Property(u => u.Type).IsRequired().HasConversion<string>();

        // EmailAddress as Owned
        builder.Entity<User>().OwnsOne(u => u.Email, email =>
        {
            email.WithOwner().HasForeignKey("Id");
            email.Property(e => e.Address).HasColumnName("Email").IsRequired();
        });


        // PostsDone
        builder.Entity<User>().OwnsOne(u => u.PostsDone, pd =>
        {
            pd.WithOwner().HasForeignKey("Id");
            pd.Property(p => p.PostIds)
                .HasColumnName("PostIds")
                .HasConversion(intListToStringConverter)
                .Metadata.SetValueComparer(intListComparer);

        });

        // Fan (no properties)
        builder.Entity<User>().OwnsOne(u => u.Fan, fan =>
        {
            fan.WithOwner().HasForeignKey("Id");
        });

        // Artist and CreatedConcerts
        builder.Entity<User>().OwnsOne(u => u.Artist, artist =>
        {
            artist.WithOwner().HasForeignKey("Id");
            artist.Property(a => a.HasCreatedConcerts)
                .HasColumnName("HasCreatedConcerts")
                .IsRequired();

            artist.OwnsOne(a => a.CreatedConcerts, cc =>
            {
                cc.WithOwner().HasForeignKey("Id");
                cc.Property(c => c.ConcertIds)
                  .HasColumnName("CreatedConcertIds")
                  .HasConversion(intListToStringConverter);
            });
        });
        


// LikedPosts como Owned Type
        builder.Entity<User>().OwnsOne(u => u.LikedPosts, lp =>
        {
            lp.WithOwner().HasForeignKey("Id");
            lp.Property(p => p.PostIds)
                .HasColumnName("LikedPosts")
                .HasConversion(intListToStringConverter)
                .Metadata.SetValueComparer(intListComparer);
        });
        
        // Relación User ↔ ConcertAttendees (tabla intermedia)
        builder.Entity<User>()
            .HasMany(u => u.ConcertLinks)
            .WithOne(ca => ca.User)
            .HasForeignKey(ca => ca.UserId);

// Relación User ↔ CommunityMembers (tabla intermedia)
        builder.Entity<User>()
            .HasMany(u => u.CommunityLinks)
            .WithOne(cm => cm.User)
            .HasForeignKey(cm => cm.UserId);



        
    }
}