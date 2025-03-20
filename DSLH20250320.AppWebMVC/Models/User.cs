using System;
using System.Collections.Generic;

namespace DSLH20250320.AppWebMVC.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public byte? Status { get; set; }
}
