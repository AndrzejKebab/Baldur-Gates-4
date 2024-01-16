using UnityEngine;
using UnityEngine.AI;

namespace GrzegorzGora.BaldurGate
{
	public class Character : MonoBehaviour
	{
		[SerializeField] private GameObject selectedIndicator; 
		[SerializeField] private CharacterData characterData;
		private NavMeshAgent navMeshAgent;
		public CharacterData CharacterData { get { return characterData; } set { if (characterData == null) characterData = value; } }
		private bool canFollow = true;
		public bool GetFollow => canFollow;
		private Vector3 followTarget;
		public bool IsSelected { get; private set; }

		private void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			InputManager.Instance.FollowClick += ChangeFollow;
			selectedIndicator.SetActive(false);
			navMeshAgent.speed = characterData.MoveSpeed;
			navMeshAgent.angularSpeed = characterData.TurnSpeed;
		}

		private void Update()
		{
			Follow();
		}

		private void Follow()
		{
			if (canFollow && followTarget == null)
			{
				navMeshAgent.SetDestination(followTarget);
			}
		}

		public void ChangeSelect(bool selected)
		{
			selectedIndicator.SetActive(selected);
			IsSelected = selected;
		}

		public void Move(Vector3 position)
		{
			navMeshAgent.SetDestination(position);
		}

		public void SetFollowTarget(Character target)
		{
			followTarget = target.transform.position;
		}

		public void ChangeFollow()
		{
			canFollow ^= true;
		}

	}
}