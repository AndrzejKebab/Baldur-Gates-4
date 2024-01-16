using PatataStudio.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterManager : PersistentSingleton<CharacterManager>, IDataPersistence
	{
		private List<Character> selectedCharacters = new();
		public List<Character> SelectedCharacters {  get { return selectedCharacters; } }
		private List<Character> allCharacters = new();
		private List<CharacterPortrait> charactersPortraits = new();
		[Header("Characters Settings")]
		[SerializeField] private Character characterPrefab;
		[SerializeField] private CharacterPortrait characterPortraitPrefab;
		[SerializeField] private RectTransform characterPortraitContent;
		[field:SerializeField] public CharacterData[] CharacterDatas { get; private set; }
		[Header("Map Generator Ref")]
		[SerializeField] private MapGenerator mapGenerator;

		private void OnDestroy()
		{
			foreach (var characterPortrait in charactersPortraits)
			{
				characterPortrait.CharacterSelect -= SelectCharacter;
				characterPortrait.CharacterDeselect -= DeselectCharacter;
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
				_newCharacter.CharacterData = characterData;
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
			character.ChangeFollow(false);
			Debug.Log($"Selected:{character.CharacterData.name}");
		}

		public void DeselectCharacter(Character character)
		{
			selectedCharacters.Remove(character);
			character.ChangeSelect(false);
			character.ChangeFollow(true);
			Debug.Log($"Deselected:{character.CharacterData.name}");
		}

		public void DeselectAll()
		{
			foreach(Character character in selectedCharacters)
			{
				character.ChangeSelect(false);
			}
			selectedCharacters.Clear();
		}

		public void DestroyAllCharacters()
		{
			foreach(var character in allCharacters)
			{
				Destroy(character.gameObject);
			}
			foreach(var portrait in charactersPortraits)
			{
				portrait.CharacterSelect -= SelectCharacter;
				portrait.CharacterDeselect -= DeselectCharacter;
				Destroy(portrait.gameObject);
			}
			charactersPortraits.Clear();
			allCharacters.Clear();
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
			return;
		}
		#endregion
	}
}