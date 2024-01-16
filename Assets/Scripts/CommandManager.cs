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
		private CommandInvoker commandInvoker = new();
		[SerializeField] private CharacterManager characterManager;
		private Camera mainCamera;
		[SerializeField] private LayerMask floorLayer;

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
			await commandInvoker.ExecuteCommand(commandQueue);
			isCommandExecuting = false;
		}

		private void Move()
		{
			Ray _ray = mainCamera.ScreenPointToRay(InputManager.Instance.MousePos);
			RaycastHit _hit;

			if(Physics.Raycast(_ray, out _hit, Mathf.Infinity, floorLayer))
			{
				foreach(var character in characterManager.SelectedCharacters)
				{
					commandQueue.Enqueue(MoveCommand.CreateMove(character, _hit.point));
				}
			}
		}
	}
}