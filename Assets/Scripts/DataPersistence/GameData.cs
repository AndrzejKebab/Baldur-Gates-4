using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class GameData
	{
		public byte MapSize;
		public float NoiseScale;
		public float NoiseMinimumThreshold;
		public Vector2 NoiseOffset;
		public List<string> CharacterDatasID;

		public GameData()
		{
			MapSize = (byte)Random.Range(50, 200);
			NoiseScale = Random.Range(10f, 20f);
			NoiseMinimumThreshold = Random.Range(0.15f, 0.5f);
			NoiseOffset = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
			CharacterDatasID = new List<string>();
		}
	}
}