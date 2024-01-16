using Unity.AI.Navigation;
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
		[SerializeField] private Transform content;
		[Range(50, 200)]
		[SerializeField] private byte mapSize;
		[Range(10, 20)]
		[SerializeField] private float noiseScale;
		[Range(0.15f, 0.5f)]
		[SerializeField] private float noiseMinimumThreshold;
		[SerializeField] private Vector2 noiseOffset;

		private NavMeshSurface navMeshSurface;
		private bool[,] mapGrid;
		public bool[,] MapGrid { get { return mapGrid; } }

		public bool IsMapCleared {  get { return mapGrid == null; } }
		public bool IsMapGenerated { get; private set; }

		private void Awake()
		{
			navMeshSurface = GetComponent<NavMeshSurface>();
			mapSize = (byte)Random.Range(50, 200);
			noiseScale = Random.Range(10f, 20f);
			noiseMinimumThreshold = Random.Range(0.15f, 0.5f);
			noiseOffset = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
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
			content = new GameObject().GetComponent<Transform>();
			content.SetParent(transform);
			for (byte x = 0; x < mapSize; x++)
			{
				for (byte z = 0; z < mapSize; z++)
				{					
					GameObject _object;
					if (mapGrid[x, z])
					{
						_object = Instantiate(wall, new Vector3(x, 0, z), Quaternion.identity, content);						
					}
					else
					{
						_object = Instantiate(floor, new Vector3(x, -0.5f, z), Quaternion.identity, content);
					}

					_object.name = $"X: {x}, Z: {z}";
				}
			}
			IsMapGenerated = true;
		}

		public void BuildNavMesh()
		{
			navMeshSurface.BuildNavMesh();
		}

		public void ClearMap()
		{
			navMeshSurface.RemoveData();
			Destroy(content.gameObject);
			IsMapGenerated = false;
			mapGrid = null;
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