using System;
using System.Runtime.InteropServices;
using PowerManagement;

namespace PowerStateManagement
{
    /// <summary>
    /// Wrapper for PowerManagment API
    /// </summary>
    public static class InteropPowerManagment
    {
        /// <summary>
        /// Wraps CallNtPowerInformationForRead method.
        /// </summary>
        /// <typeparam name="T">An output buffer type.</typeparam>
        /// <param name="level">Indicates power level information.</param>
        /// <param name="readOutputBuffer">The output buffer result.</param>
        public static void CallNtPowerInformationForRead<T>(PowerInformationLevel level, Action<IntPtr> readOutputBuffer)
        {
            var outputBufferSize = Marshal.SizeOf<T>();
            var outputBuffer = Marshal.AllocHGlobal(outputBufferSize);

            PowerManagementNative.CallNtPowerInformation((int) level, IntPtr.Zero, 0, outputBuffer,
                (uint) outputBufferSize);
            readOutputBuffer.Invoke(outputBuffer);

            Marshal.FreeHGlobal(outputBuffer);
        }

        public static void CallNtPowerInformationForWrite<T>(PowerInformationLevel level, Action<IntPtr> writeInputBuffer)
        {
            var inputBufferSize = Marshal.SizeOf<T>();
            var inputBuffer = Marshal.AllocHGlobal(inputBufferSize);

            writeInputBuffer.Invoke(inputBuffer);

            PowerManagementNative.CallNtPowerInformation((int)level, inputBuffer,
                (uint)inputBufferSize, IntPtr.Zero, 0);

            Marshal.FreeHGlobal(inputBuffer);
        }
    }
}
