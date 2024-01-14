using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GrzegorzGora.BaldurGate
{
	public class Character : MonoBehaviour
	{
		[SerializeField] private GameObject selectedIndicator; 
		[SerializeField] private CharacterData characterData;
		public CharacterData CharacterData { get { return characterData; } set { if (characterData == null) characterData = value; } }
		private bool canFollow = true;
		public bool GetFollow => canFollow;
		public bool IsSelected { get; private set; } 

		private void OnEnable()
		{
			InputManager.Instance.FollowClick += ChangeFollow;
			selectedIndicator.SetActive(false);
		}

		public void ChangeSelect(bool selected)
		{
			selectedIndicator.SetActive(selected);
			IsSelected = selected;
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