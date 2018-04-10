'It is a 32 bit COM, use version 32 bit of cscript.exe/wscript.exe located in C:\Windows\SysWOW64\'
dim powerManager

set powerManager = CreateObject("PowerManagementTask.COM.PowerManager")

msgbox(powerManager.GetSystemBatteryState())

