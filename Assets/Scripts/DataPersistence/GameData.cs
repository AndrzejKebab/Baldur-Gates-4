using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class GameData
	{
		public byte MapSize;
		public Dictionary<string, CharacterData> CharacterDatas;

		public GameData()
		{
			MapSize = (byte)Random.Range(50, 255);
			CharacterDatas = new Dictionary<string, CharacterData>();
		}
	}
}