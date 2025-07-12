using backendAppsWeb.Communities.Domain.Model.Commands;
using backendAppsWeb.Communities.Interfaces.Resources;

namespace backendAppsWeb.Communities.Interfaces.Transform;

public static class CreateCommunityCommandFromResourceAssembler
{
    public static CreateCommunityCommand toCommandFromResource(CreateCommunityResource resource)
    {
        return new CreateCommunityCommand(
            resource.Name,
          //  resource.MemberQuantity,
            resource.Image,
            //**
            resource.Description,
          //  resource.Tags,
            resource.Members
    
        );
    }
}