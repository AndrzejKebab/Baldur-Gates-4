using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CommandManager : MonoBehaviour
	{
		private bool isCommandExecuting;
		private bool isCommandQueueEmpty => commandQueue.Count == 0;
		private Queue<ICommand> commandQueue = new();
		[SerializeField] private CharacterManager characterManager;
		private Camera mainCamera;

		private void Update()
		{
			if (!isCommandQueueEmpty && !isCommandExecuting)
			{
				ExecuteCommand();
			}
		}

		private void OnEnable()
		{
			mainCamera = Camera.main;
			InputManager.Instance.LeftMouseClick += Move;
		}

		private async Task ExecuteCommand()
		{
			isCommandExecuting = true;
			while (commandQueue.Count > 0)
			{
				var command = commandQueue.Dequeue();
				await command.Execute();
			}
			isCommandExecuting = false;
		}

		private void Move()
		{
			Ray _ray = mainCamera.ScreenPointToRay(InputManager.Instance.MousePos);
			RaycastHit _hit;

			if(Physics.Raycast(_ray, out _hit))
			{
				Debug.Log("hit" + _hit.transform.position);
				foreach(var character in characterManager.SelectedCharacters)
				{
					commandQueue.Enqueue(MoveCommand.CreateMove(character, _hit.point));
				}
			}
		}
	}
}