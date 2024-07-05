using HotelListingAPI.Contracts;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers;
[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        this.accountsService = accountsService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserRequestModel request)
    {
        var result = await this.accountsService.CreateAsync(request);

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPost]
    [Route("authenticate")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] LoginUserRequestModel request)
    {
        var result = await this.accountsService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RefreshToken([FromBody] AuthenticateRequestModel request)
    {
        var result = await this.accountsService.VerifyRefreshToken(request);

        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}
