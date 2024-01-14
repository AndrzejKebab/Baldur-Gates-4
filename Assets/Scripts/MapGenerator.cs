using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GrzegorzGora.BaldurGate
{
	[BurstCompile(CompileSynchronously = true)]
	public class MapGenerator : MonoBehaviour, IDataPersistence
	{
		[Header("Map Prefabs")]
		[SerializeField] private GameObject floor;
		[SerializeField] private GameObject wall;
		[Header("Map settings")]
		[Range(50, 200)]
		[SerializeField] private byte mapSize;
		[Range(10, 20)]
		[SerializeField] private float noiseScale;
		[Range(0.15f, 0.5f)]
		[SerializeField] private float noiseMinimumThreshold;
		[SerializeField] private Vector2 noiseOffset;

		private bool[,] mapGrid;
		public bool[,] MapGrid { get { return mapGrid; } }
		private List<GameObject> mapObjects = new List<GameObject>();

		private void Awake()
		{
			mapSize = (byte)Random.Range(50, 200);
			noiseScale = Random.Range(10f, 20f);
			noiseMinimumThreshold = Random.Range(0.15f, 0.5f);
			noiseOffset = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
			InitializeMapGrid();
		}

		public void InitializeMapGrid()
		{
			mapGrid = new bool[mapSize, mapSize];

			for (byte x = 0; x < mapSize; x++)
			{
				for (byte z = 0; z < mapSize; z++)
				{
					mapGrid[x, z] = Noise(x, z);
				}
			}

			GenerateMap();
		}

		private void GenerateMap()
		{
			GameObject _object;
			for (byte x = 0; x < mapSize; x++)
			{
				for (byte z = 0; z < mapSize; z++)
				{					
					if (mapGrid[x, z])
					{
						_object = Instantiate(wall, new Vector3(x, 0, z), Quaternion.identity, transform);						
					}
					else
					{
						_object = Instantiate(floor, new Vector3(x, -0.5f, z), Quaternion.identity, transform);
					}

					_object.name = $"X: {x}, Z: {z}";
					mapObjects.Add(_object);
				}
			}
		}

		public void ClearMap()
		{
			mapGrid = null;
			foreach (var mapObject in mapObjects)
			{
				Destroy(mapObject);
			}
			mapObjects.Clear();
		}

		[BurstCompile]
		private bool Noise(byte x, byte z)
		{
			float _sampleX = (x / noiseScale) + noiseOffset.x; 
			float _sampleZ = (z / noiseScale) + noiseOffset.y;
			float2 _sampleXZ = new(_sampleX, _sampleZ);

			var _noise = noise.snoise(_sampleXZ);
			return _noise >= noiseMinimumThreshold;
		}

		public Vector3 FindSafePlaceToSpawn()
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

		public void SaveGame(GameData gameData)
		{
			gameData.MapSize = mapSize;
			gameData.NoiseScale = noiseScale;
			gameData.NoiseMinimumThreshold = noiseMinimumThreshold;
			gameData.NoiseOffset = noiseOffset;
		}

		public void LoadGame(GameData gameData)
		{
			mapSize = gameData.MapSize;
			noiseScale = gameData.NoiseScale;
			noiseMinimumThreshold = gameData.NoiseMinimumThreshold;
			noiseOffset = gameData.NoiseOffset;
		}
	}
}