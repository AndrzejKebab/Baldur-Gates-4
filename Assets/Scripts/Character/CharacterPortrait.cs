using System;
using UnityEngine;
using UnityEngine.UI;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterPortrait : MonoBehaviour
	{
		private Character character;
		public Character Character { get { return character; } set { if (character == null) character = value; } }
		[SerializeField] private Image portraitImage;
		[SerializeField] private Image followIndicator;
		
		public event Action<Character> CharacterSelect;
		public event Action<Character> CharacterDeselect;


		public void AssignCharacter(Character character, Action<Character> onCharacterSelect, Action<Character> onCharacterDeselect)
		{
			this.character = character;

			CharacterSelect -= onCharacterSelect;
			CharacterDeselect -= onCharacterDeselect;

			CharacterSelect += onCharacterSelect;
			CharacterDeselect += onCharacterDeselect;

			UpdatePortrait();
		}
		private void UpdatePortrait()
		{			
			portraitImage.color = character.IsSelected ? Color.white : Color.gray;
			followIndicator.color = character.GetFollow ? Color.green : Color.red;
		}

		public void PortraitButtonClick()
		{
			if (!character.IsSelected)
			{
				OnCharacterSelected();
			}
			else
			{
				OnCharacterDeselected();
			}			
		}

		private void OnCharacterSelected()
		{
			CharacterSelect?.Invoke(character);
			UpdatePortrait();
		}

		private void OnCharacterDeselected()
		{
			CharacterDeselect?.Invoke(character);
			UpdatePortrait();
		}
	}
}