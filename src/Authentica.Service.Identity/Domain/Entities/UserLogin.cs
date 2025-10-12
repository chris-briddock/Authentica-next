using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

public sealed class UserLogin : IdentityUserLogin<string>
{
    public override string LoginProvider { get; set; } = default!;
    public override string ProviderKey { get; set; } = default!;
    public override string? ProviderDisplayName { get; set; }
    public override string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}
