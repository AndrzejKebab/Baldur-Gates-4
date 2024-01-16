using PatataStudio.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace GrzegorzGora.BaldurGate
{
	public class CharacterManager : PersistentSingleton<CharacterManager>, IDataPersistence
	{
		private List<Character> allCharacters = new();
		private List<CharacterPortrait> charactersPortraits = new();
		private List<Character> previouslySelectedCharacters;
		private List<Character> selectedCharacters = new();
		public List<Character> SelectedCharacters {  get { return selectedCharacters; } }
		[Header("Characters Settings")]
		[SerializeField] private Character characterPrefab;
		[SerializeField] private CharacterPortrait characterPortraitPrefab;
		[SerializeField] private RectTransform characterPortraitContent;
		[field:SerializeField] public CharacterData[] CharacterDatas { get; private set; }
		[Header("Map Generator Ref")]
		[SerializeField] private MapGenerator mapGenerator;

		private void Start()
		{
			previouslySelectedCharacters = new List<Character>(selectedCharacters);	
		}

		private void Update()
		{			
			if(previouslySelectedCharacters != selectedCharacters && !selectedCharacters.IsNullOrEmpty())
			{
				foreach(var character in allCharacters)
				{
					character.SetFollowTarget(selectedCharacters[0]);
				}
				previouslySelectedCharacters.Clear();
				previouslySelectedCharacters.AddRange(selectedCharacters);
			}
		}

		private void OnDestroy()
		{
			foreach (var characterPortrait in charactersPortraits)
			{
				characterPortrait.CharacterSelect -= SelectCharacter;
				characterPortrait.CharacterDeselect -= DeselectCharacter;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create")]
		public void TestCreate()
		{
			CreateCharacters(CharacterDatas);
		}
#endif

		public void CreateCharacters(params CharacterData[] characterDatas)
		{
			var _safeSpawnPlace = mapGenerator.FindSafePlaceToSpawn();

			foreach (CharacterData characterData in characterDatas)
			{
				Character _newCharacter = Instantiate(characterPrefab, _safeSpawnPlace, Quaternion.identity);
				_newCharacter.CharacterData = characterData;
				allCharacters.Add(_newCharacter);
				CharacterPortrait _newCharacterPortrait = Instantiate(characterPortraitPrefab, characterPortraitContent.transform);
				charactersPortraits.Add(_newCharacterPortrait);
				_newCharacterPortrait.AssignCharacter(_newCharacter, SelectCharacter, DeselectCharacter);
#if UNITY_EDITOR
				_newCharacter.name = characterData.name;
				_newCharacterPortrait.name = $"{characterData.name} Portrait";
#endif
			}
		}

		public void SelectCharacter(Character character)
		{
			selectedCharacters.Add(character);
			character.ChangeSelect(true);
			character.ChangeFollow(false);
			Debug.Log($"Selected:{character.CharacterData.name}");
		}

		public void DeselectCharacter(Character character)
		{
			selectedCharacters.Remove(character);
			character.ChangeSelect(false);
			character.ChangeFollow(true);
			Debug.Log($"Deselected:{character.CharacterData.name}");
		}

		public void DeselectAll()
		{
			foreach(Character character in selectedCharacters)
			{
				character.ChangeSelect(false);
			}
			selectedCharacters.Clear();
		}

		public void DestroyAllCharacters()
		{
			foreach(var character in allCharacters)
			{
				Destroy(character.gameObject);
			}
			foreach(var portrait in charactersPortraits)
			{
				portrait.CharacterSelect -= SelectCharacter;
				portrait.CharacterDeselect -= DeselectCharacter;
				Destroy(portrait.gameObject);
			}
			charactersPortraits.Clear();
			allCharacters.Clear();
			selectedCharacters.Clear();
		}

		#region Data Persistence
		public void SaveGame(GameData gameData)
		{
			foreach (var character in allCharacters)
			{
				var _characterData = character.CharacterData.Id;
				if (gameData.CharacterDatasID.Contains(_characterData))
				{
					gameData.CharacterDatasID.Remove(_characterData);
				}
				gameData.CharacterDatasID.Add(_characterData);
			}
		}

		public void LoadGame(GameData gameData)
		{
			return;
		}
		#endregion
	}
}