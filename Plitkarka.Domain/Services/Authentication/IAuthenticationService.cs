﻿using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Services.Authentication;

public interface IAuthenticationService
{
    string Authenticate(User toAuthenticate);

    Guid Authorize(string token);
}
