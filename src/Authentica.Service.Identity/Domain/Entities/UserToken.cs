using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

public class UserToken : IdentityUserToken<string>
{
    public User User { get; set; } = default!;
}
