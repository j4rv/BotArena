using System.Threading;

namespace BotArena
{
    public class RobotThread
    {
        private Thread thread;
        private ThreadStart threadStart;

        //              CONSTRUCTORS

        public RobotThread()
        {
            thread = new Thread(Nothing);
            thread.Priority = ThreadPriority.Lowest;
        }
        

        //              TURN HANDLERS
        
        public bool newTurn(ThreadStart threadStart)
        {
            this.threadStart = threadStart;
            return StartNewTurn();
        }

        private bool StartNewTurn()
        {
            //If the thread is still alive, the robot loses a turn.
            if (!thread.IsAlive) { 
                thread = new Thread(threadStart);
                thread.Start();
                return true;
            }
            return false;
        }


        private void Nothing() { }
    }
}
