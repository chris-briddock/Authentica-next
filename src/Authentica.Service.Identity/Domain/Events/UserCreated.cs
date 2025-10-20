using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

public sealed class UserCreated : IDomainEvent
{
    public DateTime OccurredAt { get; }

    public UserCreated(DateTime occurredAt)
    {
        OccurredAt = occurredAt;
    }

}