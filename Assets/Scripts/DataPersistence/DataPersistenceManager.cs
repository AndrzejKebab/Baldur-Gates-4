using PatataStudio.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GrzegorzGora.BaldurGate
{
	public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
	{
		[SerializeField] private string fileName;

		private GameData gameData;
		public List<IDataPersistence> dataPersistencesObjects;
		private FileDataHandler fileDataHandler;
#if UNITY_EDITOR
		[ContextMenu("SAVE")]
		public void TestSave()
		{
			SaveGame();
		}

		[ContextMenu("LOAD")]
		public void TestLoad()
		{
			LoadGame();
		}
#endif
		protected override void Awake()
		{
			fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
			gameData = new GameData();
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			dataPersistencesObjects = FindAllDataPersistenceObjects();
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		public void LoadGame()
		{
			gameData = fileDataHandler.LoadFile();
			
			if (gameData == null)
			{
				Debug.LogWarning("No data found. Data can't be loaded.");
			}

			foreach (var dataPersistenceObject in dataPersistencesObjects)
			{
				dataPersistenceObject.LoadGame(gameData);
			}
		}

		public void SaveGame()
		{
			if (gameData == null)
			{
				Debug.LogWarning("No data found. Data can't be saved");
				return;
			}

			foreach (var dataPersistenceObject in dataPersistencesObjects)
			{
				dataPersistenceObject.SaveGame(gameData);
			}

			fileDataHandler.SaveFile(gameData);
		}

		private List<IDataPersistence> FindAllDataPersistenceObjects()
		{
			IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

			return new List<IDataPersistence>(dataPersistencesObjects);
		}
	}
}