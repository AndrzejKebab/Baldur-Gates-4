using System;
using System.IO;
using Unity.Serialization.Json;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class FileDataHandler
	{
		private string fullPath = "";

		public FileDataHandler(string dataDirPath, string dataFileName)
		{
			fullPath = Path.Combine(dataDirPath, dataFileName);
		}

		public GameData LoadFile()
		{
			GameData _loadedData = null;

			if (File.Exists(fullPath))
			{
				try
				{
					string _dataToLoad = "";
					using (FileStream stream = new FileStream(fullPath, FileMode.Open))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							_dataToLoad = reader.ReadToEnd();
						}
					}

					_loadedData = JsonSerialization.FromJson<GameData>(_dataToLoad);
				}
				catch (Exception e)
				{
					Debug.LogError("Error occured when trying to load file at path: " + fullPath + ".\n" + e);
				}
			}

			return _loadedData;
		}

		public void SaveFile(GameData gameData)
		{
			string _dataToStore = JsonSerialization.ToJson(gameData);
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

				using (FileStream stream = new FileStream(fullPath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(stream))
					{
						writer.Write(_dataToStore);
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to save data to file:" + fullPath + "\n" + e);
			}
		}
	}
}
