using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterManager : MonoBehaviour
	{
		private List<Character> selectedCharacters;
		private List<Character> allCharacters;


		public void CreateCharacter(params CharacterData[] characterDatas)
		{
			var _mapGrid = GetComponent<MapGenerator>().MapGrid;
			var _safeSpawnPlace = Vector3.zero;

			for (byte x  = 0; x < _mapGrid.GetLength(0); x++)
			{
				for (byte z = 0; z < _mapGrid.GetLength(1); z++)
				{
					if(!_mapGrid[x, z])
					{
						_safeSpawnPlace = new Vector3(x, 1, z);
						break;
					}
				}
			}

			foreach (var characterData in characterDatas)
			{
				Instantiate(characterData, _safeSpawnPlace, Quaternion.identity);
			}
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