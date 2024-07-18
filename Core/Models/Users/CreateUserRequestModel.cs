using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Models.Users;

public class CreateUserRequestModel : BaseUserModel
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }
}
