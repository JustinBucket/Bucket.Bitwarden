using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden
{
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}