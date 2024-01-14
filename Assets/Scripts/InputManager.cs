using PatataStudio.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static GrzegorzGora.BaldurGate.PlayerInputActions;

namespace GrzegorzGora.BaldurGate
{
	[RequireComponent(typeof(PlayerInput))]
	public class InputManager : PersistentSingleton<InputManager>, IPlayerActions
	{
		private PlayerInputActions playerInput;
		public Vector2 MousePos { get; private set; }
		public event Action LeftMouseClick;
		public event Action FollowClick;

		private void Start()
		{
			playerInput = new();
			playerInput.Player.Enable();
			playerInput.Player.SetCallbacks(this);
		}

		private void OnDestroy()
		{
			playerInput.Player.RemoveCallbacks(this);
		}

		public void OnLeftClick(InputAction.CallbackContext context)
		{
			if(context.performed) LeftMouseClick?.Invoke();
		}

		public void OnFollow(InputAction.CallbackContext context)
		{
			if(context.performed) FollowClick?.Invoke();
		}

		public void OnCursorPosition(InputAction.CallbackContext context) => MousePos = context.ReadValue<Vector2>();
	}
}