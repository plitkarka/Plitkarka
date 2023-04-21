﻿using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.AuthControllerModels;

public record VerifyEmailRequestModel
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email length should be less or equal 100 symbols")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [StringLength(6, MinimumLength = 6, ErrorMessage = "Email code should be exactly 6 digits")]
    [Required(ErrorMessage = "Email is required")]
    public string EmailCode { get; set; }
}