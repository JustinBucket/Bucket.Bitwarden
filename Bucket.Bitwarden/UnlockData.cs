using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden
{
    public class UnlockData
    {
        public bool NoColor { get; set; }
        public string Object { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Raw { get; set; }
    }
}