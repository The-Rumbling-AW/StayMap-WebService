using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Model.Commands;
using backendAppsWeb.Communities.Domain.Model.Entity;
using backendAppsWeb.Communities.Domain.Repositories;
using backendAppsWeb.Communities.Domain.Services;
using backendAppsWeb.Communities.Domain.Services.Command;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.Communities.Application.CommandServices;

public class CommunityCommandService(
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork) : ICommunityCommandService
{
    public async Task<Community?> Handle(CreateCommunityCommand command)
    {
        var createdCommunity = new Community(
            command.Name,
        //    command.MemberQuantity,
            command.Image,
            command.Description
        //    command.Tags,
            //command.Members
           
        );

        try
        {
            await communityRepository.AddAsync(createdCommunity);
            await unitOfWork.CompleteAsync();
            return createdCommunity;
        }
        catch (Exception ex)
        {
            // log opcional: Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public async Task<bool> Handle(DeleteCommunityCommand command)
    {
        var community = await communityRepository.FindByIdAsync(command.Id);
        if (community == null) return false;

        communityRepository.Delete(community);
        await unitOfWork.CompleteAsync();
        return true;
    }
    
    
    public async Task<bool> Handle(JoinCommunityCommand command)
    {
        var community = await communityRepository.FindDetailedByIdAsync(command.CommunityId);
        if (community == null) return false;

        if (community.UserLinks.Any(link => link.UserId == command.UserId))
            return false; // ya está unido

        community.UserLinks.Add(new CommunityMember
        {
            CommunityId = command.CommunityId,
            UserId = command.UserId
        });

        await unitOfWork.CompleteAsync();
        return true;
    }
    public async Task<bool> Handle(LeaveCommunityCommand command)
    {
        var community = await communityRepository.FindDetailedByIdAsync(command.CommunityId);
        if (community == null) return false;

        var existingLink = community.UserLinks.FirstOrDefault(link => link.UserId == command.UserId);
        if (existingLink == null) return false; // No estaba unido

        community.UserLinks.Remove(existingLink);
        await communityRepository.UpdateAsync(community);
        await unitOfWork.CompleteAsync();
        return true;
    }
    
}