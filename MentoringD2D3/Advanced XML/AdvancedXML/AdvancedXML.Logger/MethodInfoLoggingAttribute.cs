using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using PostSharp.Aspects;

namespace AdvancedXML.Logger
{
    [Serializable]
    public class MethodInfoLoggingAttribute : OnMethodBoundaryAspect
    {
        private IMethodInfoLogger methodInfoLogger;

        private IMethodInfoLogger MethodInfoLogger
        {
            get
            {
                if(methodInfoLogger == null)
                    InitializeLogger();
                return methodInfoLogger;
            }
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (!args.Method.IsConstructor)
            {
                MethodInfoLogger.LogEntry(
                    args.Method.DeclaringType.Name,
                    args.Method.Name,
                    GetParameters(args));
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (!args.Method.IsConstructor)
            {
                MethodInfoLogger.LogExit(
                    args.Method.DeclaringType.Name,
                    args.Method.Name,
                    args.ReturnValue);
            }
        }

        private void InitializeLogger()
        {
            methodInfoLogger = new MethodInfoLogger();
        }


        private Dictionary<string, object> GetParameters(MethodExecutionArgs args)
        {
            return args.Method.GetParameters()
                .Select((x, i) => new { ParamName = x.Name, ParamValue = args.Arguments[i] })
                .ToDictionary(x => x.ParamName, x => x.ParamValue);
        }
    }
}
