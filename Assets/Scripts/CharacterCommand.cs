using System.Threading.Tasks;

namespace GrzegorzGora.BaldurGate
{
	public abstract class CharacterCommand : ICommand
	{
		protected readonly Character character;

		protected CharacterCommand(Character character)
		{
			this.character = character;
		}

		public abstract Task Execute();
		public static T Create<T>(Character character) where T : Character
		{
			return (T) System.Activator.CreateInstance(typeof(T), character);
		}
	}
}