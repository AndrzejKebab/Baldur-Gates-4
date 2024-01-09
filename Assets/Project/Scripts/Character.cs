using System;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class Character : MonoBehaviour
	{
		public CharacterData characterData;
		private bool canFollow = true;

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

		public bool GetFollow() => canFollow;
	}
}