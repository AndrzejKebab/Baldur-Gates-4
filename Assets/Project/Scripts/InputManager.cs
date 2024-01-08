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

		public Vector2 MousePos => selectAction.ReadValue<Vector2>();
		public event Action LeftMouseClick;

		private void Start()
		{
			playerInput = GetComponent<PlayerInput>();
			selectAction = playerInput.actions["Select"];
			leftClickAction = playerInput.actions["LeftClick"];

			leftClickAction.performed += OnLeftClick;
		}

		private void OnDestroy()
		{
			leftClickAction.performed -= OnLeftClick;
		}

		private void OnLeftClick(InputAction.CallbackContext obj) => LeftMouseClick?.Invoke();
	}
}