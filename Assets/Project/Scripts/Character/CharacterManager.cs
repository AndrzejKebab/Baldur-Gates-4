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

		[SerializeField] private GameObject characterPortraitPrefab;
		private GameObject characterPortraitContent;
		public CharacterData[] CharacterDatas;

		private void Awake()
		{
			characterPortraitContent = GameObject.Find("Canvas/CharacterPortraits/Content");
		}

		[ContextMenu("Create")]
		public void Test()
		{
			CreateCharacters(CharacterDatas);
		}

		public void CreateCharacters(params CharacterData[] characterDatas)
		{
			var _mapGrid = GameObject.Find("Map").GetComponent<MapGenerator>().MapGrid;
			if (_mapGrid == null)
			{
				Debug.LogError("Map or Map Generator not Found!");
				return;
			}

			var _safeSpawnPlace = FindSafePlaceToSpawn(_mapGrid);

			foreach (CharacterData characterData in characterDatas)
			{
				Character _newCharacter = new GameObject().AddComponent<Character>();
				_newCharacter.characterData = characterData;
				_newCharacter.transform.position = _safeSpawnPlace;
				allCharacters.Add(_newCharacter);
				CharacterPortrait _newCharacterPortrait = Instantiate(characterPortraitPrefab, characterPortraitContent.transform).GetComponentInChildren<CharacterPortrait>();
				_newCharacterPortrait.Character = _newCharacter.GetComponent<Character>();
				_newCharacterPortrait.SetPortrait();
				_newCharacterPortrait.CharacterSelect += SelectCharacters;
				_newCharacterPortrait.CharacterDeselect += DeselectCharacters;
			}
		}

		private Vector3 FindSafePlaceToSpawn(bool[,] mapGrid)
		{
			for (byte x = 0; x < mapGrid.GetLength(0); x++)
			{
				for (byte z = 0; z < mapGrid.GetLength(1); z++)
				{
					if (!mapGrid[x, z])
					{
						return new Vector3(x, 1, z);
					}
				}
			}

			Debug.LogWarning("Safe Place not found! Returning default!");
			return default;
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
				var _characterData = character.characterData;
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