using System;
using UnityEngine;
using UnityEngine.UI;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterPortrait : MonoBehaviour
	{
		public Character Character;
		private Image portraitImage;
		private Image followIndicator;
		
		public event Action<Character> CharacterSelect;
		public event Action<Character> CharacterDeselect;

		public void SetPortrait()
		{
			if (Character == null)
			{
				Debug.LogError("Character reference in CharacterPortrait is null. Unable to create portrait!");
				return;
			}
			
			followIndicator = GetComponentInChildren<Image>();
			followIndicator.color = Character.GetFollow() ? Color.green : Color.red;
		}
	}
}