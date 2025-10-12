using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.ValueObjects;

namespace Authentica.Service.Identity.Domain.Entities;


public sealed class UserMultiFactorSettings : UserMultiFactorSettings<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; } = default!;

    public User User { get; set; } = default!;
}   

public abstract class UserMultiFactorSettings<TKey> : IEntityCreationStatus<string>,
                                                      IEntityModificationStatus<string> 
    where TKey : IEquatable<TKey> 
{
    public virtual TKey Id { get; set; } = default!;

    public virtual bool IsEmailEnabled { get; set; }

    public virtual bool IsPhoneEnabled { get; set; }

    public virtual bool IsAuthenticatorEnabled { get; set; }

    public virtual bool IsPasskeysEnabled { get; set; }

    public virtual string ConcurrencyStamp { get; set; } = default!;
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;
}
