using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrzegorzGora.BaldurGate
{
	public class CommandInvoker
	{
		public async Task ExecuteCommand(Queue<ICommand> commands)
		{
			foreach (var command in commands)
			{
				await command.Execute();
			}
		}
	}
}