using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/New Data")]
	public class CharacterData : ScriptableObject
	{
		public string Id;
		public float MoveSpeed;
		public float TurnSpeed;
		public float Stamina;

		[ContextMenu("Generate Id")]
		private void GenerateGuid()
		{
			Id = System.Guid.NewGuid().ToString();
		}
	}
}