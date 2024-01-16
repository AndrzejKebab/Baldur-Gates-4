using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/New Data")]
	public class CharacterData : ScriptableObject
	{
		public string Id;
		[Range(1f, 8f)]
		public float MoveSpeed;
		[Range(60f, 180f)]
		public float TurnSpeed;
		[Range(5f, 15f)]
		public float Stamina;

		private void OnValidate()
		{
			if (string.IsNullOrWhiteSpace(Id))
			{
				GenerateGuid();
				RandomizeValues();
			}
		}

		[ContextMenu("Generate Id")]
		private void GenerateGuid()
		{
			Id = System.Guid.NewGuid().ToString();
		}

		[ContextMenu("Randomize Values")]
		private void RandomizeValues()
		{
			MoveSpeed = Random.Range(1f, 8f);
			TurnSpeed = Random.Range(60f, 180f);
			Stamina = Random.Range(5f, 15f);
		}
	}
}