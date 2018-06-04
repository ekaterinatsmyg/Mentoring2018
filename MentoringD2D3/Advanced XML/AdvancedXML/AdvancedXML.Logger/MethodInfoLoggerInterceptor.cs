using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace AdvancedXML.Logger
{
    public class MethodInfoLoggerInterceptor : IInterceptor
    {
        private readonly IMethodInfoLogger methodInfoLogger;

        public MethodInfoLoggerInterceptor(IMethodInfoLogger methodInfoLogger)
        {
            this.methodInfoLogger = methodInfoLogger;
        }

        public void Intercept(IInvocation invocation)
        {
            PreTrace(invocation);
            invocation.Proceed();
            PostTrace(invocation);
        }

        private void PreTrace(IInvocation invocation)
        {
            methodInfoLogger.LogEntry(
                invocation.TargetType.Name,
                invocation.MethodInvocationTarget.Name,
                GetParameters(invocation));
        }

        private void PostTrace(IInvocation invocation)
        {
            methodInfoLogger.LogExit(
                invocation.TargetType.Name,
                invocation.MethodInvocationTarget.Name,
                invocation.ReturnValue);
        }
        
        private Dictionary<string, object> GetParameters(IInvocation invocation)
        {
            return invocation.MethodInvocationTarget.GetParameters()
                .Select((x, i) => new { ParamName = x.Name, ParamValue = invocation.Arguments[i] })
                .ToDictionary(x => x.ParamName, x => x.ParamValue);
        }
    }
}
