using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Auth;

public class AgrigateAuthContext: IdentityDbContext<AgrigateUser>
{
    /// <summary>
    /// Parameterless constructor for migrations
    /// </summary>
    public AgrigateAuthContext()
    {
    }
    
    public AgrigateAuthContext(DbContextOptions<AgrigateAuthContext> options) : base(options)
    {
    }
}