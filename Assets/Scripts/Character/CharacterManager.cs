using System;
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

		[SerializeField] private CharacterPortrait characterPortraitPrefab;
		[SerializeField] private GameObject characterPortraitContent;
		public CharacterData[] CharacterDatas;

		[SerializeField] private MapGenerator mapGenerator;

		private void OnDestroy()
		{
			foreach (var characterPortrait in charactersPortraits)
			{
				characterPortrait.CharacterSelect -= SelectCharacters;
				characterPortrait.CharacterSelect -= DeselectCharacters;
			}
		}

		[ContextMenu("Create")]
		public void TestCreate()
		{
			CreateCharacters(CharacterDatas);
		}

		public void CreateCharacters(params CharacterData[] characterDatas)
		{
			var _safeSpawnPlace = mapGenerator.FindSafePlaceToSpawn();

			foreach (CharacterData characterData in characterDatas)
			{
				Character _newCharacter = new GameObject().AddComponent<Character>();
				_newCharacter.CharacterData = characterData;
				_newCharacter.transform.position = _safeSpawnPlace;
				_newCharacter.name = characterData.name;
				allCharacters.Add(_newCharacter);
				CharacterPortrait _newCharacterPortrait = Instantiate(characterPortraitPrefab, characterPortraitContent.transform);
				charactersPortraits.Add(_newCharacterPortrait);
				_newCharacterPortrait.AssignCharacter(_newCharacter, SelectCharacters, DeselectCharacters);
				_newCharacterPortrait.name = $"{characterData.name} Portrait";
			}
		}

		public void SelectCharacters(Character character)
		{
			throw new NotImplementedException();
		}

		public void DeselectCharacters(Character character)
		{
			throw new NotImplementedException();
		}

		public void SaveGame(GameData gameData)
		{
			foreach (var character in allCharacters)
			{
				var _characterData = character.CharacterData;
				if (gameData.CharacterDatas.ContainsKey(_characterData.Id))
				{
					gameData.CharacterDatas.Remove(_characterData.Id);
				}
				gameData.CharacterDatas.Add(_characterData.Id, _characterData);
			}
		}

		public void LoadGame(GameData gameData)
		{
			CharacterData[] _characterDatas = gameData.CharacterDatas.Values.ToArray();

			CreateCharacters(_characterDatas);
		}
	}
}