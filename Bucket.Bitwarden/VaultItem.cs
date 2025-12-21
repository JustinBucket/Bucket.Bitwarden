using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bucket.Bitwarden.Get
{
    public class VaultItem
    {
        // not sure what this actually looks like
        // public string[] PasswordHistory { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Object { get; set; }
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public int Type { get; set; }
        public int Reprompt { get; set; }
        public string Name { get; set; }    
        public string Notes { get; set; }
        [JsonProperty("favorite")]
        public bool Favourite { get; set; }
        public Login Login { get; set; }
        // have an idea what this is - think it's a <string, string> dictionary
        // public string[] Fields { get; set; }
    }
}