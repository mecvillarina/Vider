using System;

namespace Domain.Enums
{
    [Flags]
    public enum NFTFlag
    {
        Burnable = 1,
        OnlyXRP = 2,
        TrustLine = 4,
        Transferable = 8,
    }
}
