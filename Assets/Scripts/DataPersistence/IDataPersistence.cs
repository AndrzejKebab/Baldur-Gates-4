namespace GrzegorzGora.BaldurGate
{
	public interface IDataPersistence
	{
		public void SaveGame(GameData gameData);
		public void LoadGame(GameData gameData);
	}
}
