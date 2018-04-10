using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement
{
    /// <summary>
    /// Contains information about the idleness of the system.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemPowerInformation
    {
        uint MaxIdlenessAllowed;
        uint Idleness;
        uint TimeRemaining;
        byte CoolingMode;

        public override string ToString() => $"MaxIdlenessAllowed: {MaxIdlenessAllowed}, Idleness: {Idleness}, " +
                                             $"TimeRemaining: {TimeSpan.FromTicks(TimeRemaining)}, CoolingMode: {CoolingMode}";
    }
}
