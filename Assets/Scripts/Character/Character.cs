using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GrzegorzGora.BaldurGate
{
	public class Character : MonoBehaviour
	{
		private CharacterData characterData;
		public CharacterData CharacterData { get { return characterData; } set { if (characterData == null) characterData = value; } }
		private bool canFollow = true;
		public bool GetFollow() => canFollow;
		private void OnEnable()
		{
			InputManager.Instance.FollowClick += ChangeFollow;
		}

		private void OnDisable()
		{
			InputManager.Instance.FollowClick -= ChangeFollow;
		}

		public void Move(Vector2 position)
		{
			throw new NotImplementedException();
		}

		public void Follow(Character target)
		{
			throw new NotImplementedException();
		}

		public void ChangeFollow()
		{
			canFollow ^= true;
		}

	}
}