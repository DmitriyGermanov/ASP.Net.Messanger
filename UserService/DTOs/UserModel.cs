﻿using UserService.Models;

namespace UserService.DTOs
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public virtual string? RoleName { get; set; }
    }
}
