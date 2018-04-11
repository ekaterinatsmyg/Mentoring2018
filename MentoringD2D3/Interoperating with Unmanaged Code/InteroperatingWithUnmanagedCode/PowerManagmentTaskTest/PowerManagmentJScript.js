'It is a 32 bit COM, use version 32 bit of cscript.exe/wscript.exe located in C:\Windows\SysWOW64\'
var powerManager = new ActiveXObject("PowerManagementTask.COM.PowerManager");
WScript.Echo(powerManager.GetSystemBatteryState());