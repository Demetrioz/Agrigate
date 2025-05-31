using Microsoft.AspNetCore.Identity;
using Agrigate.Domain.Entities.Common;

namespace Agrigate.Web.Services;

internal sealed class IdentityUserAccessor(UserManager<AgrigateUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<AgrigateUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}
