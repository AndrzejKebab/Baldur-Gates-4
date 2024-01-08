using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	[BurstCompile(CompileSynchronously = true)]
	public class MapGenerator : MonoBehaviour
	{
		[SerializeField] private GameObject floor;
		[SerializeField] private GameObject wall;
		[SerializeField] private Material floorMaterial;
		[SerializeField] private Material wallMaterial;
		[SerializeField] private byte mapSize;
		public bool[,] MapGrid;

		private void Awake()
		{
			MapGrid = new bool[mapSize, mapSize];	
		}

		void Start()
		{
			InitializeMapGrid();
		}

		private void InitializeMapGrid()
		{
			for (byte x = 0; x < mapSize; x++)
			{
				for (byte z = 0; z < mapSize; z++)
				{
					MapGrid[x, z] = Noise(x, z);
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
					if (MapGrid[x, z])
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
			float _scale = 20;
			float _sampleX = x / _scale - 1; 
			float _sampleZ = z / _scale;
			float2 _sampleXZ = new float2(_sampleX, _sampleZ);

			var _noise = noise.snoise(_sampleXZ);
			
			if(_noise >= 0.25f)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}