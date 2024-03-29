﻿using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Authentication;

public record SignInRequest(
    string Email,
    string Password,
    string UniqueIdentifier)
    : IRequest<TokenPairResponse>;