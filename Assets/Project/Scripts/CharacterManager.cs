using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterManager : MonoBehaviour
	{
		private List<Character> selectedCharacters;
		private List<Character> allCharacters;

		public CharacterData[] CharacterDatas;

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
				var _newCharacter = new GameObject();
				_newCharacter.AddComponent<Character>().characterData = characterData;
				_newCharacter.transform.position = _safeSpawnPlace;
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

			Debug.LogError("Safe Place not found! Returning default!");
			return default;
		}

		public List<Character> SelectCharacters()
		{
			throw new NotImplementedException();
		}

		public void DeselectCharacters()
		{
			throw new NotImplementedException();
		}
	}
}