using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Services;

public class AccountsService : IAccountsService
{
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;

    public AccountsService(
        IMapper mapper,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.configuration = configuration;
    }
    public async Task<IdentityResult> CreateAsync(CreateUserRequestModel request)
    {
        var user = mapper.Map<User>(request);
        user.UserName = request.Email;

        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
        }

        return result;
    }

    public async Task<AuthenticateResponseModel?> LoginAsync(LoginUserRequestModel request)
    {
        var user = await this.userManager.FindByEmailAsync(request.Email);
        var isValidUser = await this.userManager.CheckPasswordAsync(user, request.Password);

        if(user is null || isValidUser == false)
        {
            return null;
        }

        var token = await GenerateToken(user);

        var result = new AuthenticateResponseModel
        {
            UserId = user.Id,
            Token = token,
        };

        return result;
    }

    private async Task<string> GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Authentication:Key"] ?? String.Empty));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await this.userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

        var userClaims = await this.userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),

        }
        .Union(roleClaims)
        .Union(userClaims);

        var token = new JwtSecurityToken(
            issuer: this.configuration["Authentication:Issuer"],
            audience: this.configuration["Authentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(this.configuration["Authentication:DurationInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
 