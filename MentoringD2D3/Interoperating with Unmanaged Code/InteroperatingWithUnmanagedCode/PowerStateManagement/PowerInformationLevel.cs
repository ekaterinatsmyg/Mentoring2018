﻿namespace PowerManagement
{
    /// <summary>
    /// Indicates power level information.
    /// </summary>
    public enum PowerInformationLevel
    {
        SystemPowerPolicyAc,
        SystemPowerPolicyDc,
        VerifySystemPolicyAc,
        VerifySystemPolicyDc,
        SystemPowerCapabilities,
        SystemBatteryState,
        SystemPowerStateHandler,
        ProcessorStateHandler,
        SystemPowerPolicyCurrent,
        AdministratorPowerPolicy,
        SystemReserveHiberFile,
        ProcessorInformation,
        SystemPowerInformation,
        ProcessorStateHandler2,
        LastWakeTime,
        LastSleepTime,
        SystemExecutionState,
        SystemPowerStateNotifyHandler,
        ProcessorPowerPolicyAc,
        ProcessorPowerPolicyDc,
        VerifyProcessorPowerPolicyAc,
        VerifyProcessorPowerPolicyDc,
        ProcessorPowerPolicyCurrent,
        SystemPowerStateLogging,
        SystemPowerLoggingEntry,
        SetPowerSettingValue,
        NotifyUserPowerSetting,
        PowerInformationLevelUnused0,
        PowerInformationLevelUnused1,
        SystemVideoState,
        TraceApplicationPowerMessage,
        TraceApplicationPowerMessageEnd,
        ProcessorPerfStates,
        ProcessorIdleStates,
        ProcessorCap,
        SystemWakeSource,
        SystemHiberFileInformation,
        TraceServicePowerMessage,
        ProcessorLoad,
        PowerShutdownNotification,
        MonitorCapabilities,
        SessionPowerInit,
        SessionDisplayState,
        PowerRequestCreate,
        PowerRequestAction,
        GetPowerRequestList,
        ProcessorInformationEx,
        NotifyUserModeLegacyPowerEvent,
        GroupPark,
        ProcessorIdleDomains,
        WakeTimerList,
        SystemHiberFileSize,
        PowerInformationLevelMaximum
    }
}