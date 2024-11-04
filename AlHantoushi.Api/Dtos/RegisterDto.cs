﻿using System.ComponentModel.DataAnnotations;

namespace AlHantoushi.Api.Dtos;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
