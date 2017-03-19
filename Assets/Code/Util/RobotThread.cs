using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace BotArena
{
    public class RobotThread
    {
        private Thread thread;
        private ThreadStart turn;

        //              CONSTRUCTORS

        public RobotThread()
        {
            thread = new Thread(Nothing);
            thread.Priority = ThreadPriority.Lowest;
        }

        public RobotThread(RobotThreadShadedData robotData)
        {
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        //              TURN HANDLERS

        public void newTurn(ThreadStart turn)
        {
            this.turn = turn;
            StartNewTurn();
        }

        public void StartNewTurn()
        {
            //If the thread is still alive, the robot loses a turn.
            if (!thread.IsAlive) { 
                thread = new Thread(turn);
                thread.Start();
            }
        }


        private void Nothing() { }
    }
}
