using System.Collections.Generic;

namespace AdvancedXML.Logger
{
    public interface IMethodInfoLogger
    {
        void LogEntry(string type, string methodName, Dictionary<string, object> parameters);

        void LogExit(string type, string methodName, object returnValue);
    }
}
