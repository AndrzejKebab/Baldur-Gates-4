using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrzegorzGora.BaldurGate
{
	public class CommandInvoker
	{
		public async Task ExecuteCommand(Queue<ICommand> commands)
		{
			while (commands.Count > 0)
			{
				var command = commands.Dequeue();
				await command.Execute();
			}
		}
	}
}