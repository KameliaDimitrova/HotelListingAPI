using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Services;

public class AccountsService : IAccountsService
{
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private User user;

    private const string refreshToken = "RefreshToken";
    private const string tokenProvider = "HotelListingApi";

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
        if (user != null)
        {
            this.user = user;
        }
        var isValidUser = await this.userManager.CheckPasswordAsync(this.user, request.Password);

        if(user is null || isValidUser == false)
        {
            return null;
        }

        var token = await GenerateToken();

        var result = new AuthenticateResponseModel
        {
            UserId = user.Id,
            Token = token,
            RefreshToken = await CreateRefreshTokenAsync()
        };

        return result;
    }

    private async Task<string> GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Authentication:Key"] ?? String.Empty));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await this.userManager.GetRolesAsync(this.user);

        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

        var userClaims = await this.userManager.GetClaimsAsync(this.user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, this.user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, this.user.Email),

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

    public async Task<AuthenticateResponseModel?> VerifyRefreshToken(AuthenticateRequestModel request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
        var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
        if(username is null)
        {
            return null;
        }
        var user = await userManager.FindByNameAsync(username);

        if(user is null || user.Id != request.UserId)
        {
            return null;
        }
        this.user = user;
        var isValidRefreshToken = await this.userManager.VerifyUserTokenAsync(this.user, tokenProvider, refreshToken, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateToken();
            return new AuthenticateResponseModel
            {
                Token = token,
                UserId = this.user.Id,
                RefreshToken = await CreateRefreshTokenAsync()
            };
        }

        await this.userManager.UpdateSecurityStampAsync(this.user);

        return null;
    }

    private async Task<string> CreateRefreshTokenAsync()
    {
        await this.userManager.RemoveAuthenticationTokenAsync(this.user, tokenProvider, "RefreshToken");
        var newRefreshToken = await this.userManager.GenerateUserTokenAsync(this.user, tokenProvider, refreshToken);
        await this.userManager.SetAuthenticationTokenAsync(this.user, tokenProvider, refreshToken, newRefreshToken);

        return newRefreshToken;
    }
}
 