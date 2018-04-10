using System.Runtime.InteropServices;

namespace PowerManagement
{
    /// <summary>
    /// Contains information about the current state of the system battery.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemBatteryState
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool AcOnLine;
        [MarshalAs(UnmanagedType.I1)]
        public bool BatteryPresent;
        [MarshalAs(UnmanagedType.I1)]
        public bool Charging;
        [MarshalAs(UnmanagedType.I1)]
        public bool Discharging;
        public byte Spare1;
        public byte Spare2;
        public byte Spare3;
        public byte Spare4;
        public uint MaxCapacity;
        public uint RemainingCapacity;
        public uint Rate;
        public uint EstimatedTime;
        public uint DefaultAlert1;
        public uint DefaultAlert2;

        public override string ToString() => $"AcOnLine: {AcOnLine}, BatteryPresent: {BatteryPresent}, " +
                                             $"Charging: {Charging}, Discharging: {Discharging}, " +
                                             $"Spare1 {string.Join("|", Spare1)}, " +
                                             $"Spare2 {string.Join("|", Spare2)}, " +
                                             $"Spare3 {string.Join("|", Spare3)}, " +
                                             $"Spare4 {string.Join("|", Spare4)}, " +
                                             $"MaxCapacity: {MaxCapacity}, RemainingCapacity: {RemainingCapacity}, " +
                                             $"Rate: {Rate}, EstimatedTime: {EstimatedTime}, " +
                                             $"DefaultAlert1: {DefaultAlert1}, DefaultAlert2: {DefaultAlert2}";
    }
}
