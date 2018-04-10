using System;
using System.Runtime.InteropServices;
using PowerManagement;
using PowerStateManagement;

namespace PowerManagementTask
{
    /// <summary>
    /// Provides Power Managment .Net API 
    /// </summary>
    public static class PowerManager
    {
        /// <summary>
        /// Gets a current system battery state.
        /// </summary>
        /// <returns>A current system battery state.</returns>
        public static SystemBatteryState GetSystemBatteryState()
        {
            return GetPowerInformation<SystemBatteryState>(PowerInformationLevel.SystemBatteryState);
        }

        /// <summary>
        /// Gets a power information of teh system.
        /// </summary>
        /// <returns>A power information of teh system.</returns>
        public static SystemPowerInformation GetSystemPowerInformation()
        {
            return GetPowerInformation<SystemPowerInformation>(PowerInformationLevel.SystemPowerInformation);
        }

        /// <summary>
        /// Gets a last time of the system sleep.
        /// </summary>
        /// <returns>A last system sleep time</returns>
        public static DateTime GetLastSleepTime()
        {
            var lastSleepDuration = GetPowerInformation<long>(PowerInformationLevel.LastSleepTime);

            var lastSleepTime = DateTime.Now - TimeSpan.FromMilliseconds(PowerManagementNative.GetTickCount64()) +
                       TimeSpan.FromTicks(lastSleepDuration);

            return lastSleepTime;
        }

        /// <summary>
        /// Gets a last time of the system wake.
        /// </summary>
        /// <returns>A last system wake time</returns>
        public static DateTime GetLastWakeTime()
        {
            var lastWakeDuration = GetPowerInformation<long>(PowerInformationLevel.LastWakeTime);

            var lastSleepTime = DateTime.Now - TimeSpan.FromMilliseconds(PowerManagementNative.GetTickCount64()) +
                                TimeSpan.FromTicks(lastWakeDuration);

            return lastSleepTime;
        }

        /// <summary>
        /// Suspends the system by shutting power down in a sleep state.
        /// </summary>
        public static void SleepSystem()
        {
            PowerManagementNative.SetSuspendState(false, false, false);
        }

        /// <summary>
        /// Suspends the system by shutting power down in hibernation.
        /// </summary>
        public static void HybernateSystem()
        {
            PowerManagementNative.SetSuspendState(true, false, false);
        }

        /// <summary>
        /// Commits the storage required to hold the hibernation image on the boot volume.
        /// </summary>
        public static void ReserveHibernationFile()
        {
            HandleHibernationFile(true);
        }

        /// <summary>
        /// Decommits the storage required to hold the hibernation image on the boot volume.
        /// </summary>
        public static void RemoveHibernationFile()
        {
            HandleHibernationFile(false);
        }

        /// <summary>
        /// Commits or decommits depending on <value>isReseving</value> the storage required to hold the hibernation image on the boot volume.
        /// </summary>
        /// <param name="isReseving">A value that indecates if the hibernate file should be removed or reserved.</param>
        private static void HandleHibernationFile(bool isReseving)
        {
           //InteropPowerMnagment.CallNtPowerInformationForWrite<int>(PowerInformationLevel.SystemReserveHiberFile, buffer =>
           // {
           //    Marshal.WriteInt32(buffer, Convert.ToInt32(isReseving));
           // });
            var inputBufferSize = Marshal.SizeOf<bool>();
            var inputBuffer = Marshal.AllocHGlobal(inputBufferSize);

            Marshal.StructureToPtr(isReseving, inputBuffer, false);

            //Marshal.WriteInt32(inputBuffer, 0, isReseving ? 1 : 0);
            PowerManagementNative.CallNtPowerInformation((int)PowerInformationLevel.SystemReserveHiberFile, inputBuffer, (uint)inputBufferSize, IntPtr.Zero, 0);

            Marshal.FreeHGlobal(inputBuffer);
        }

        /// <summary>
        /// Gets a value that indicates the remaining battery life 
        /// (as a percentage of the full battery charge). 
        /// This value is in the range 0-100, 
        /// where 0 is not charged and 100 is fully charged.  
        /// </summary>
        public static int GetBatteryLifePercent()
        {
            if (!GetSystemBatteryState().BatteryPresent)
                return 100;

            var state = GetSystemBatteryState();

            int percent = (int)Math.Round((double)state.RemainingCapacity / state.MaxCapacity * 100, 0);
            return percent;
        }

        /// <summary>
        /// Gets System Power Information by PowerInformation level.
        /// </summary>
        /// <typeparam name="T">An output buffer type.</typeparam>
        /// <param name="powerLevel">Indicates power level information.</param>
        /// <returns>The output buffer result.</returns>
        private static T GetPowerInformation<T>(PowerInformationLevel powerLevel)
        {
            T powerInformation = default(T);

            InteropPowerManagment.CallNtPowerInformationForRead<SystemBatteryState>(
                powerLevel,
                buffer => powerInformation = Marshal.PtrToStructure<T>(buffer));

            return powerInformation;
        }

    }
}
