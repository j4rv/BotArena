using System.Threading;

namespace BotArena
{
    class RobotThread
    {
        Thread thread;
        ThreadStart threadStart;

        //              CONSTRUCTORS

        public RobotThread() {
            thread = new Thread(Nothing);
            thread.Priority = ThreadPriority.Lowest;
        }


        //              TURN HANDLERS

        public bool newTurn(ThreadStart threadStart) {
            this.threadStart = threadStart;
            return StartNewTurn();
        }

        bool StartNewTurn() {
            //If the thread is still alive, the robot loses a turn.
            if (!thread.IsAlive) {
                thread = new Thread(threadStart);
                thread.Start();
                return true;
            }
            return false;
        }


        void Nothing() { 
            // Empty method for the constructor
        }
    }
}
