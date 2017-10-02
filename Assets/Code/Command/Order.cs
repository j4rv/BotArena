using System.Collections.Generic;
using System.Linq;

//http://stackoverflow.com/questions/1360533/how-to-share-data-between-different-threads-in-c-sharp-using-aop
//https://msdn.microsoft.com/en-us/library/aa645740(VS.71).aspx#vcwlkthreadingtutorialexample1creating
namespace BotArena
{
    public class Order
    {
        readonly RobotController controller;
        bool executed;
        SortedList<Command, ICommand> commands;
        int turn;
		static object orderLock = new object();


        internal Order(RobotController robotController, int currentTurn) {
            commands = new SortedList<Command, ICommand>();
            controller = robotController;
            turn = currentTurn;
        }

		/// <summary>
		/// Returns the turn in which this command was created.
		/// </summary>
		/// <returns>The turn as an int.</returns>
        public int GetTurn()
		{
			lock (orderLock) {
				return turn;
			}
		}

		/// <summary>
		/// Adds the command.
		/// </summary>
		/// <returns><c>true</c>, if command was added, <c>false</c> otherwise.</returns>
		/// <param name="cmd">The command.</param>
		/// <param name="args">Arguments for the command (speed, rotation...).</param>
		public bool AddCommand(Command cmd, params object[] args)
		{
			lock (orderLock) {
				bool res = commands.Remove(cmd);
				ICommand command = CommandFactory.Create(cmd, controller, args);
				commands.Add(cmd, command);

				return res;
			}
		}

		/// <returns><c>true</c>, if command was removed, <c>false</c> otherwise.</returns>
		/// <param name="cmd">Cmd.</param>
		public bool RemoveCommand(Command cmd)
		{
			lock (orderLock) {
				return commands.Remove(cmd);
			}
		}

		/// <summary>
		/// Reset the commands list.
		/// </summary>
		public void Reset()
		{
			lock (orderLock) {
				commands.Clear();
			}
		}

        internal bool IsExecuted() {
			lock (orderLock) {
                return executed;
			}
        }

        internal void Executed() {
			lock (orderLock){
                executed = true;
			}
        }


        internal List<ICommand> GetCommands() {
			lock (orderLock) {
                return commands.Values.ToList();
			}
        }
    }
}
