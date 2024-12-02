using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_mvc.Models.Authentication.User
{
    public class TokenType
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiryTokenDate { get; set; }
    }
}