using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.TaskMulth
{
    public class ThreadState
    {
        public EventWaitHandle eventWaitHandle = new ManualResetEvent(false);

        public int Result {get; set;}
    }
}
