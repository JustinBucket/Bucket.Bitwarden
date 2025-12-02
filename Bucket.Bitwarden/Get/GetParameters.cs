using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden
{
    public struct GetParameters
    {
        public string ItemName;
        public Guid ItemId;

        public GetParameters(string itemName)
        {
            ItemName = itemName;
        }

        public GetParameters(Guid itemId)
        {
            ItemId = itemId;
            ItemName = "";
        }

        public string GenerateQueryString()
        {
            if (ItemId != Guid.Empty)
            {
                return $"item {ItemId}";
            }
            else if (!string.IsNullOrWhiteSpace(ItemName))
            {
                return $"item \"{ItemName}\"";
            }
            else
            {
                throw new ArgumentException("Either ItemId or ItemName must be provided.");
            }
        }
    }
}