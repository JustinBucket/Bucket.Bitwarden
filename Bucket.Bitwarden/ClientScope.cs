using System.ComponentModel;

namespace Bucket.Bitwarden
{
    public enum ClientScope
    {
        Invalid,
        [Description("api.organization")]
        ApiOrganization
    }
}