using System.Threading.Tasks;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class MoveCommand : CharacterCommand
	{
		private Vector3 destination;
		private MoveCommand(Character character, Vector3 destination) : base(character)
		{
			this.destination = destination;
		}

		public static MoveCommand CreateMove(Character character, Vector3 destination)
		{
			return new MoveCommand(character, destination);
		}

		public override async Task Execute()
		{
			character.Move(destination);
			await Task.Delay(0);
		}
	}
}
