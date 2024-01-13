using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterManager : MonoBehaviour, IDataPersistence
	{
		private List<Character> selectedCharacters = new();
		private List<Character> allCharacters = new();
		private List<CharacterPortrait> charactersPortraits = new();

		[SerializeField] private Character characterPrefab;
		[SerializeField] private CharacterPortrait characterPortraitPrefab;
		[SerializeField] private RectTransform characterPortraitContent;
		[SerializeField] private CharacterData[] CharacterDatas;

		[SerializeField] private MapGenerator mapGenerator;

		private void OnDestroy()
		{
			foreach (var characterPortrait in charactersPortraits)
			{
				characterPortrait.CharacterSelect -= SelectCharacter;
				characterPortrait.CharacterSelect -= DeselectCharacter;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create")]
		public void TestCreate()
		{
			CreateCharacters(CharacterDatas);
		}
#endif

		public void CreateCharacters(params CharacterData[] characterDatas)
		{
			var _safeSpawnPlace = mapGenerator.FindSafePlaceToSpawn();

			foreach (CharacterData characterData in characterDatas)
			{
				Character _newCharacter = Instantiate(characterPrefab, _safeSpawnPlace, Quaternion.identity);
				_newCharacter.name = characterData.name;
				allCharacters.Add(_newCharacter);
				CharacterPortrait _newCharacterPortrait = Instantiate(characterPortraitPrefab, characterPortraitContent.transform);
				charactersPortraits.Add(_newCharacterPortrait);
				_newCharacterPortrait.AssignCharacter(_newCharacter, SelectCharacter, DeselectCharacter);
				_newCharacterPortrait.name = $"{characterData.name} Portrait";
			}
		}

		public void SelectCharacter(Character character)
		{
			selectedCharacters.Add(character);
			character.ChangeSelect(true);
		}

		public void DeselectCharacter(Character character)
		{
			selectedCharacters.Remove(character);
			character.ChangeSelect(false);
		}

		public void DeselectAll()
		{
			selectedCharacters.Clear();
		}

		#region Data Persistence
		public void SaveGame(GameData gameData)
		{
			foreach (var character in allCharacters)
			{
				var _characterData = character.CharacterData;
				if (gameData.CharacterDatas.Contains(_characterData))
				{
					gameData.CharacterDatas.Remove(_characterData);
				}
				gameData.CharacterDatas.Add(_characterData);
			}
		}

		public void LoadGame(GameData gameData)
		{
			CharacterData[] _characterDatas = gameData.CharacterDatas.ToArray();
			if (_characterDatas.IsNullOrEmpty()) return;
			CreateCharacters(_characterDatas);
		}
		#endregion
	}
}