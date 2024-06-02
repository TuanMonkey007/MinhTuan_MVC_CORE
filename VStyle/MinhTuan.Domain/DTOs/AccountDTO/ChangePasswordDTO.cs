﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.AccountDTO
{
    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class ChangePasswordDTO
    {
        public string NewPassword { get; set; }
        public string Password { get; set; }

    }
}
