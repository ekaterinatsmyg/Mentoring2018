var powerManager = new ActiveXObject("PowerManagementTask.COM.PowerManager");
WScript.Echo(powerManager.GetSystemBatteryState());