using System;

namespace Health
{
    [Flags]
    public enum DeathCauses
    {
        External,
        Internal,
        Spikes,
        Acid,
        Door
    }
}