﻿namespace Hospital.Application.Models.Auth
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Session { get; set; }
    }
}
