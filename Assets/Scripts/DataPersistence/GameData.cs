public class GameData
{
    public SerializableDateTime saveDate = new();
    public int unlockedLevels;
    public int currentLevel;
    public int[] starsObtained;
    
    public GameData()
    {
        unlockedLevels = 1;
        currentLevel = 1;
        starsObtained = new int[] {0, 0, 0, 0, 0};
    }
    
}
