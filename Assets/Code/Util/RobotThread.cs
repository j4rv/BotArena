using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace BotArena
{
    public class RobotThread
    {
        private Thread thread;
        private List<ThreadStart> jobs;

        public RobotThread()
        {
            jobs = new List<ThreadStart>();
            thread = new Thread(Nothing);
            thread.Priority = ThreadPriority.BelowNormal;
        }

        public RobotThread(ThreadStart function)
        {
            thread = new Thread(function);
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        public void NewJob(ThreadStart job)
        {
            jobs.Add(job);
            WaitAndStartNextJob();
        }

        public void WaitAndStartNextJob()
        {
            while (thread.IsAlive) { }

            ThreadStart job = jobs.Last();
            jobs.Remove(job);

            thread = new Thread(job);
            thread.Start();
        }

        private void Nothing() { }
    }
}
