using System.Collections.Generic;
using System.Threading;
using System.Linq;

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

        public void newTurn(ThreadStart threadStart)
        {
            this.threadStart = threadStart;
            StartNewTurn();
        }

        private void StartNewTurn()
        {
            //If the thread is still alive, the robot loses a turn.
            if (!thread.IsAlive) { 
                thread = new Thread(threadStart);
                thread.Start();
            }
        }


        private void Nothing() { }
    }
}
