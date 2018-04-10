using System.Runtime.InteropServices;

namespace PowerManagementTask.COM
{
    [ComVisible(true)]
    [Guid("2FFBF138-793B-44ED-94E9-BFBEC518A5A8")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerManager : IPowerManager
    {
        public int GetBatteryLifePer()
        {
            return PowerManagementTask.PowerManager.GetBatteryLifePercent();
        }

        public string GetSystemBatteryState()
        {
            return PowerManagementTask.PowerManager.GetSystemBatteryState().ToString();
        }
    }
}
