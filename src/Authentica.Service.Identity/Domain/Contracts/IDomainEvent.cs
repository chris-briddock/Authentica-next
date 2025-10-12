namespace Authentica.Service.Identity.Domain.Contracts;

public interface IDomainEvent
{
   public DateTime OccurredAt { get; }
}
