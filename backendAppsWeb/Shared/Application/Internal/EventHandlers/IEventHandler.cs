using backendAppsWeb.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace backendAppsWeb.Shared.Infrastructure.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> 
where TEvent : IEvent
{
    
}