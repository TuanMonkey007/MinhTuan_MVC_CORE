﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.AccountDTO
{
    public class RegisterDTO
    {
        public  string? UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string  Email { get; set; }
        public string Password { get; set; }
    }
}