using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.ValueObjects;

namespace Authentica.Service.Identity.Domain.Entities;

public sealed class Session : Session<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}

public abstract class Session<TKey> : IEntityCreationStatus<string>
    where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; } = default!;
    public virtual string IpAddress { get; set; } = default!;
    public virtual string UserAgent { get; set; } = default!;

    public virtual string Status { get; set; } = default!;
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

}
