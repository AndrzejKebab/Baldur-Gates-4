using Cinemachine;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CameraFollowCharacter : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera virtualCamera;

		private void LateUpdate()
		{
			var _target = CharacterManager.Instance.SelectedCharacters;
			if (_target.IsNullOrEmpty()) return;
			virtualCamera.Follow = _target[0].transform;
		}
	}
}