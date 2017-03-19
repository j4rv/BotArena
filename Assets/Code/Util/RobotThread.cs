using System.Threading;

namespace BotArena
{
    public class RobotThread
    {
        private Thread thread;

        public RobotThread()
        {
            thread = new Thread(Nothing);
            thread.Priority = ThreadPriority.BelowNormal;
        }

        public RobotThread(ThreadStart function)
        {
            thread = new Thread(function);
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        public void NewJob(ThreadStart function)
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(function);
                thread.Start();
            }
        }

        private void Nothing() { }
    }
}
