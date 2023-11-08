public class GameData
{
    public SerializableDateTime saveDate = new();
    public int unlockedLevels;
    public int currentLevel;
    
    public GameData()
    {
        unlockedLevels = 1;
        currentLevel = 1;
    }
    
}
