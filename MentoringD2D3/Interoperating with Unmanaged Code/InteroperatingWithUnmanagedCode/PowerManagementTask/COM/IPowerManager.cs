using System.Runtime.InteropServices;

namespace PowerManagementTask.COM
{
    [ComVisible(true)]
    [Guid("A000837A-D25F-4985-BCEA-DBA1BAEDA9D9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IPowerManager
    {
        int GetBatteryLifePer();

        string GetSystemBatteryState();
    }
}
