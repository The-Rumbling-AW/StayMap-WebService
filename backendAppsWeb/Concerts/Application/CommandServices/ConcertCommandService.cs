using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.Commands;
using backendAppsWeb.Concerts.Domain.Model.Entity;
using backendAppsWeb.Concerts.Domain.Repositories;
using backendAppsWeb.Concerts.Domain.Services;
using backendAppsWeb.Concerts.Domain.Services.Command;
using backendAppsWeb.IAM.Domain.Repositories;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.Concerts.Application.CommandServices;

public class ConcertCommandService(
    IConcertRepository concertRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IConcertCommandService
{
    public async Task<Concert?> Handle(CreateConcertCommand command)
    {
        var currentDate = DateTime.Now;
        if (command.Date < currentDate)
        {
            throw new ArgumentException("Date cannot be in the past");
        }

        var providedAttendeeIds = (command.Attendees ?? new List<int>())
            .Where(id => id > 0)
            .ToList();

        if (providedAttendeeIds.Any())
        {
            var existingUserIds = await userRepository.GetExistingUserIdsAsync(providedAttendeeIds);
            var nonExisting = providedAttendeeIds.Except(existingUserIds).ToList();

            if (nonExisting.Any())
                throw new ArgumentException($"Los siguientes usuarios no existen: {string.Join(", ", nonExisting)}");
        }

        var createdConcert = new Concert(
            command.Name,
            command.Genre,
            command.Date,
            command.Description,
            command.Image,
            command.NameVenue,
            command.Address,
            command.Latitude,
            command.Longitude,
            command.Capacity,
            command.Platform // 👈 nuevo
        );

        // 👇 Asociar los asistentes
        foreach (var userId in providedAttendeeIds)
        {
            createdConcert.AttendeeLinks.Add(new ConcertAttendee
            {
                UserId = userId,
                Concert = createdConcert
            });
        }

        try
        {
            await concertRepository.AddAsync(createdConcert);
            await unitOfWork.CompleteAsync();
            return createdConcert;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> Handle(DeleteConcertCommand command)
    {
        var concert = await concertRepository.FindByIdAsync(command.Id);
        if (concert == null) return false;

        concertRepository.Delete(concert);
        await unitOfWork.CompleteAsync();
        return true;
    }
    
    public async Task<bool> Handle(AttendConcertCommand command)
    {
        try
        {
            var concert = await concertRepository.FindDetailedByIdAsync(command.ConcertId);
            if (concert == null) return false;

            if (concert.AttendeeLinks.Any(a => a.UserId == command.UserId))
                return false;

            concert.AttendeeLinks.Add(new ConcertAttendee
            {
                ConcertId = command.ConcertId,
                UserId = command.UserId
            });

            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en AttendConcertCommandHandler: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> ToggleAttendance(int concertId, int userId)
    {
        var concert = await concertRepository.FindDetailedByIdAsync(concertId);
        if (concert == null) return false;

        var existingAttendee = concert.AttendeeLinks
            .FirstOrDefault(a => a.UserId == userId);

        if (existingAttendee != null)
        {
            // ✅ Ya existe: eliminarlo (cancelar asistencia)
            concert.AttendeeLinks.Remove(existingAttendee);
        }
        else
        {
            // ✅ No existe: agregarlo (confirmar asistencia)
            concert.AttendeeLinks.Add(new ConcertAttendee
            {
                ConcertId = concertId,
                UserId = userId
            });
        }

        await unitOfWork.CompleteAsync();
        return true;
    }
    
    public async Task<bool> CancelAttendance(int concertId, int userId)
    {
        var concert = await concertRepository.FindDetailedByIdAsync(concertId);
        if (concert == null) return false;

        var existing = concert.AttendeeLinks.FirstOrDefault(a => a.UserId == userId);
        if (existing == null) return false;

        concert.AttendeeLinks.Remove(existing);
        await unitOfWork.CompleteAsync();

        return true;
    }
    public async Task<bool> Handle(UpdateConcertStatusCommand command)
    {
        var concert = await concertRepository.FindByIdAsync(command.ConcertId);
        if (concert == null) return false;

        concert.Status = command.NewStatus;
        await unitOfWork.CompleteAsync();
        return true;
    }

    
}
