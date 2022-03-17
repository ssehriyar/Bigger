using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int seed;
    public int boardSize;
    public int gameObjectNumber;
    public int currentLevel;
    public List<GameObjectData> gameObjects;

    public GameData(int currentLevel)
	{
        this.seed = RandomUtil.Instance.Range(0, int.MaxValue);
        RandomUtil.Instance.Seed(this.seed);

        this.boardSize = RandomUtil.Instance.Range(4, 7);
        this.gameObjectNumber = RandomUtil.Instance.Range(5, 11);
        this.currentLevel = currentLevel;
        gameObjects = new List<GameObjectData>();
    }

    public void SetExistingData(GameData gameData)
	{
        RandomUtil.Instance.Seed(gameData.seed);
        RandomUtil.Instance.Range(4, 7);
        RandomUtil.Instance.Range(4, 7);
        this.currentLevel = gameData.currentLevel;
    }
}
