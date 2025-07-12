using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.Entity;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backendAppsWeb.Concerts.Infrastructure.Persistence.EFC.Configurations.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyConcertConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concert>().HasKey(c => c.Id);

        modelBuilder.Entity<Concert>().Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        modelBuilder.Entity<Concert>().Property(c => c.Name)
            .HasMaxLength(30)
            .IsRequired();

        modelBuilder.Entity<Concert>().Property(c => c.Genre)
            .IsRequired()
            .HasConversion<string>(); 

        modelBuilder.Entity<Concert>().Property(c => c.Date)
            .IsRequired();

        modelBuilder.Entity<Concert>().Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>(); 

        modelBuilder.Entity<Concert>().Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        modelBuilder.Entity<Concert>().Property(c => c.Image)
            .IsRequired();


        modelBuilder.Entity<Venue>().HasKey(v => v.Id);

        modelBuilder.Entity<Venue>().Property(v => v.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        
        //venue
       
        modelBuilder.Entity<Venue>().Property(v => v.Namevenue).IsRequired();
        modelBuilder.Entity<Venue>().Property(v => v.Address).IsRequired();
        modelBuilder.Entity<Venue>().Property(v => v.Capacity).IsRequired();
        
        modelBuilder.Entity<Venue>()
            .HasMany(v => v.Concerts)
            .WithOne(c => c.Venue)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Venue>().OwnsOne(l => l.Location, lo=>
        {
            lo.WithOwner()
                    .HasForeignKey("Id");
            lo.Property(l => l.Latitude).HasColumnName("VenueLatitude").IsRequired();
            lo.Property(l => l.Longitude).HasColumnName("VenueLongitude").IsRequired();
        });
        
        modelBuilder.Entity<ConcertAttendee>()
            .ToTable("concertattendees") // 👈 nombre explícito de la tabla intermedia
            .HasKey(ca => new { ca.ConcertId, ca.UserId });

        modelBuilder.Entity<ConcertAttendee>()
            .HasOne(ca => ca.Concert)
            .WithMany(c => c.AttendeeLinks)
            .HasForeignKey(ca => ca.ConcertId);

        modelBuilder.Entity<ConcertAttendee>()
            .HasOne(ca => ca.User)
            .WithMany(p => p.ConcertLinks)
            .HasForeignKey(ca => ca.UserId);
        
        modelBuilder.Entity<Concert>().Property(c => c.Platform)
            .IsRequired()
            .HasConversion<string>(); // almacena como texto legible en la base de datos



        
    }
}