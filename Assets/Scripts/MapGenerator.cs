using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	[BurstCompile(CompileSynchronously = true)]
	public class MapGenerator : MonoBehaviour, IDataPersistence
	{
		[Header("Map Prefabs")]
		[SerializeField] private GameObject floor;
		[SerializeField] private GameObject wall;
		[SerializeField] private Material floorMaterial;
		[SerializeField] private Material wallMaterial;
		[Header("Map settings")]
		[SerializeField] private byte mapSize;
		[SerializeField] private byte noiseScale;
		[SerializeField] private float noiseMinimumThreshold;
		[SerializeField] private Vector2 noiseOffset;

		private bool[,] mapGrid;
		public bool[,] MapGrid { get { return mapGrid; } }

		void Start()
		{
			InitializeMapGrid();
		}

		private void InitializeMapGrid()
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
			for (byte x = 0; x < mapSize; x++)
			{
				for (byte z = 0; z < mapSize; z++)
				{
					GameObject _object;
					if (mapGrid[x, z])
					{
						_object = Instantiate(wall, new Vector3(x, 0, z), Quaternion.identity, transform);
					}
					else
					{
						_object = Instantiate(floor, new Vector3(x, -0.5f, z), Quaternion.identity, transform);
					}

					_object.name = $"X: {x}, Z: {z}";
				}
			}
		}

		[BurstCompile]
		private bool Noise(byte x, byte z)
		{
			float _sampleX = x / noiseScale + noiseOffset.x; 
			float _sampleZ = z / noiseScale + noiseOffset.y;
			float2 _sampleXZ = new float2(_sampleX, _sampleZ);

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
		}

		public void LoadGame(GameData gameData)
		{
			mapSize = gameData.MapSize;
		}
	}
}