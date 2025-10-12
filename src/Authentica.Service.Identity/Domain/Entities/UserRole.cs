using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

/// <inheritdoc/>
public sealed class UserRole : IdentityUserRole<string>
{
    public User User { get; set; } = default!;

    public Role Role { get; set; } = default!;
}
