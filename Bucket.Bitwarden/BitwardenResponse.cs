using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden
{
    public class BitwardenResponse<T>
    {
        public bool Sucess { get; set; }
        public T Data { get; set; }
    }
}