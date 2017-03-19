using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//http://stackoverflow.com/questions/1360533/how-to-share-data-between-different-threads-in-c-sharp-using-aop
//https://msdn.microsoft.com/en-us/library/aa645740(VS.71).aspx#vcwlkthreadingtutorialexample1creating
namespace BotArena
{
    public class Order
    {
        private RobotController controller;
        private bool executed;
        private SortedList<Command, ICommand> commands;


        public Order(RobotController controller)
        {
            commands = new SortedList<Command, ICommand>();
            this.controller = controller;
        }

        public bool IsExecuted()
        {
            lock (this)
                return executed;
        }

        public void Executed()
        {
            lock (this)
                executed = true;
        }

        public List<ICommand> GetCommands()
        {
            lock (this)
                return commands.Values.ToList();
        }

        /// Returns true if a command was overriden
        public bool AddCommand(Command cmd, params object[] args)
        {
            lock (this) { 
                bool res = commands.Remove(cmd);
                ICommand command = CommandFactory.Create(cmd, controller, args);
                commands.Add(cmd, command);
            

                return res;
            }
        }

        public void Reset()
        {
            lock (this)
                commands.Clear();
        }
    }
}
