﻿using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Core.Contracts;

public interface IAccountsService : IService
{
    Task<IdentityResult> CreateAsync(CreateUserRequestModel request);

    Task<AuthenticateResponseModel?> LoginAsync(LoginUserRequestModel request);

    Task<AuthenticateResponseModel?> VerifyRefreshToken(AuthenticateRequestModel request);
}
