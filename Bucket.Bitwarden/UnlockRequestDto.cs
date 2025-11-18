using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden
{
    public class UnlockRequestDto
    {
        public string Password { get; set; }
        public UnlockRequestDto(string password)
        {
            Password = password;
        }
    }
}