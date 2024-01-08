using System;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class Character : MonoBehaviour
	{
		public CharacterData characterData;
		private bool canFollow;

		public void Move(Vector2 position)
		{
			throw new NotImplementedException();
		}

		public void Follow(Character target)
		{
			throw new NotImplementedException();
		}
	}
}