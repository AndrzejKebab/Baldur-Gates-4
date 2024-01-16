using System.Threading.Tasks;

namespace GrzegorzGora.BaldurGate
{
	public interface ICommand
	{
		public Task Execute();
	}
}