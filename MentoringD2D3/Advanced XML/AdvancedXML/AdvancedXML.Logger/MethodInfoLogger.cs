using System.Collections.Generic;
using AdvancedXML.Diagnostics;
using Newtonsoft.Json;
using NLog;

namespace AdvancedXML.Logger
{
    public class MethodInfoLogger : IMethodInfoLogger
    {
        private const string NotSerializableErrorMsg = "Parametrs are not serializable";
        
        public void LogEntry(string type, string methodName, Dictionary<string, object> parameters)
        {
            var serializedArgs = GetSerializedObject(parameters);
            ApplicationLogger.LogMessage(LogMessageType.Trace, $"Method started: {type}.{methodName} with following arguments: {serializedArgs}");
        }

        public void LogExit(string type, string methodName, object returnValue)
        {
            var serializedReturnValue = GetSerializedObject(returnValue);
            ApplicationLogger.LogMessage(LogMessageType.Trace, $"Method completed: {type}.{methodName}. Return values: {serializedReturnValue}");
        }

        private string GetSerializedObject(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch
            {
                return NotSerializableErrorMsg;
            }
        }
    }
}
