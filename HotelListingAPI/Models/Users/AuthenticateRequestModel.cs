namespace HotelListingAPI.Models.Users;

public class AuthenticateRequestModel : BaseUserModel
{
    public required string UserId { get; set; }

    public required string Token { get; set; }

    public required string RefreshToken { get; set; }
}
