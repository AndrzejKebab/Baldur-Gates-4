using PatataStudio.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrzegorzGora.BaldurGate
{
	[RequireComponent(typeof(PlayerInput))]
	public class InputManager : Singleton<InputManager>
	{
		private PlayerInput playerInput;
		private InputAction selectAction;
		private InputAction leftClickAction;
		private InputAction followAction;

		public Vector2 MousePos => selectAction.ReadValue<Vector2>();
		public event Action LeftMouseClick;
		public event Action FollowClick;

		private void Start()
		{
			playerInput = GetComponent<PlayerInput>();
			selectAction = playerInput.actions["Select"];
			leftClickAction = playerInput.actions["LeftClick"];
			followAction = playerInput.actions["Follow"];

			leftClickAction.performed += OnLeftClick;
			followAction.performed += OnFollowClick;
		}

		private void OnDestroy()
		{
			leftClickAction.performed -= OnLeftClick;
			followAction.performed -= OnFollowClick;
		}

		private void OnLeftClick(InputAction.CallbackContext obj) => LeftMouseClick?.Invoke();
		private void OnFollowClick(InputAction.CallbackContext obj) => FollowClick?.Invoke();
	}
}