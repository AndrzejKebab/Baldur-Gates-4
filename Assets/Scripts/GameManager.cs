using PatataStudio.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
	{
		[SerializeField] private MapGenerator mapGenerator;
		[SerializeField] private CharacterManager characterManager;
		private bool isGameInitialized = false;

		private void Start()
		{
			List<CharacterData> _characterDatas = new();
			for (byte i = 0; i < (byte)Random.Range(1, characterManager.CharacterDatas.Length + 1); i++)
			{
				_characterDatas.Add(characterManager.CharacterDatas[i]);
			}
			StartCoroutine(InitializeGame(_characterDatas.ToArray()));
		}

		private void Update()
		{
			if(isGameInitialized)
			{
				StopCoroutine(InitializeGame());
			}
		}

		private IEnumerator InitializeGame(params CharacterData[] characterDatas)
		{
			isGameInitialized = false;
			if(mapGenerator.IsMapCleared && !mapGenerator.IsMapGenerated)
			{
				mapGenerator.InitializeMapGrid();
				yield return null;
			}
			if(mapGenerator.IsMapGenerated && !mapGenerator.IsMapCleared)
			{
				mapGenerator.BuildNavMesh();
				characterManager.DestroyAllCharacters();
				characterManager.CreateCharacters(characterDatas);
				yield return null;
			}
			isGameInitialized = true;
		}

		public void LoadGame(GameData gameData)
		{
			CharacterData[] _characterDatas = gameData.CharacterDatas.ToArray();
			mapGenerator.ClearMap();
			StartCoroutine(InitializeGame(_characterDatas));
		}

		public void SaveGame(GameData gameData)
		{
			return;
		}
	}
}