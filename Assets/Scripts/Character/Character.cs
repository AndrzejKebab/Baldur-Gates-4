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
		private Character followTarget;
		public bool IsSelected { get; private set; }

		private void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
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
			if(followTarget == null)
			{
				if (CharacterManager.Instance.SelectedCharacters.Count == 0) return;
				Character _target = CharacterManager.Instance.SelectedCharacters[0];
				if (_target == this) return;
				followTarget = _target;
			}

			if (canFollow && !IsSelected)
			{
				Move(followTarget.transform.position);
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
			if(target == this) return;
			followTarget = target;
		}

		public void ChangeFollow(bool canFollow)
		{
			this.canFollow = canFollow;
		}

	}
}