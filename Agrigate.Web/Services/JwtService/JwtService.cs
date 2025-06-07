using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Agrigate.Core.Configuration;
using Agrigate.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Agrigate.Web.Services.JwtService;

/// <inheritdoc />
public class JwtService : IJwtService
{
    private ILogger<JwtService> _logger;
    private readonly UserManager<AgrigateUser> _userManager;
    private readonly AuthenticationConfiguration _configuration;

    public JwtService(
        ILogger<JwtService> logger,
        IOptions<AuthenticationConfiguration> options,
        UserManager<AgrigateUser> userManager
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }
    
    /// <inheritdoc />
    public async Task<string> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("User not found for {Email}", email);
            return "";
        }

        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", user.Id),
            new("name", user.UserName ?? ""),
            new("email", user.Email ?? ""),
        };

        // TODO: Implement Roles - NotSupportedException: Store does not implement IUserRoleStore<TUser>
        // var roles = await _userManager.GetRolesAsync(user);
        // if (roles.Count > 0)
        //     claims.Add(new Claim("roles", JsonSerializer.Serialize(roles)));

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_configuration.TokenDurationMinutes),
            signingCredentials: signingCredentials
        );
        
        var handler = new JwtSecurityTokenHandler();
        var tokenString = handler.WriteToken(tokenOptions);
        
        return tokenString;
    }
}