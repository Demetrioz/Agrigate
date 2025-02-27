using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Auth;

public class AgrigateAuthContext(DbContextOptions<AgrigateAuthContext> options)
    : IdentityDbContext<AgrigateUser>(options)
{
}