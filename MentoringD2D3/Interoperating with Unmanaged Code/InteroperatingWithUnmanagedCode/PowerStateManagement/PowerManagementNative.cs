using System;
using System.Runtime.InteropServices;

namespace PowerManagement
{
    /// <summary>
    /// Importer of the unmanaged library methods
    /// </summary>
    public static class PowerManagementNative
    {

        /// <summary>
        /// Sets or retrieves power information.
        /// </summary>
        /// <param name="informationLevel">The value indicates the specific power information to be set or retrieved.</param>
        /// <param name="inputBuffer">A pointer to an optional input buffer.</param>
        /// <param name="inputBufferSize">The size of the input buffer, in bytes.</param>
        /// <param name="outputBuffer">A pointer to an optional output buffer.</param>
        /// <param name="outputBufferSize">The size of the output buffer, in bytes.</param>
        /// <returns>If the function succeeds, the return value is STATUS_SUCCESS.</returns>
        [DllImport("powrprof.dll")]
        public static extern uint CallNtPowerInformation(
            int informationLevel,
            IntPtr inputBuffer,
            uint inputBufferSize,
            [Out] IntPtr outputBuffer,
            uint outputBufferSize);

        /// <summary>
        /// Suspends the system by shutting power down. 
        /// Depending on the Hibernate parameter, the system either enters a suspend (sleep) state or hibernation (S4).
        /// </summary>
        /// <param name="hibernate">If this parameter is TRUE, the system hibernates. If the parameter is FALSE, the system is suspended.</param>
        /// <param name="forceCritical"></param>
        /// <param name="disableWakeEvent">If this parameter is TRUE, the system disables all wake events. If the parameter is FALSE, any system wake events remain enabled.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("powrprof.dll")]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        /// <summary>
        /// Retrieves the number of milliseconds that have elapsed since the system was started.
        /// </summary>
        /// <returns>The number of milliseconds.</returns>
        [DllImport("kernel32.dll")]
        public static extern ulong GetTickCount64();

    }
}
