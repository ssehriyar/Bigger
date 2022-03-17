using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePersist : MonoBehaviour
{
    public static GamePersist Instance { get; private set; }
    
    GameData _gameData;
    public int CurrentLevel { get; set; } = 1;

    public delegate void LoadingFinished();
    public static LoadingFinished loadFinished;

    public delegate void CreatingObjects(int boardSize, int gameObjectNumber);
    public static CreatingObjects creatingObjects;

    void Awake()
    {
        if (Instance != this && Instance != null)
            DestroyImmediate(this);
        else
            Instance = this;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        DontDestroyOnLoad(this.gameObject);
        StartScene(IsGameDataLoaded());
    }

    void OnLevelFinishedLoading(Scene _, LoadSceneMode __) => StartScene(false);

    void StartScene(bool isGameDataLoaded)
    {
        if (!isGameDataLoaded)
        {
            _gameData = new GameData(CurrentLevel);
        }

        creatingObjects?.Invoke(_gameData.boardSize, _gameData.gameObjectNumber);

        if (isGameDataLoaded)
        {
            foreach (var gameObjectPiece in FindObjectsOfType<GameObjectPiece>(includeInactive: true))
            {
                var gameObjectData = _gameData.gameObjects.FirstOrDefault(t => t.Name == gameObjectPiece.name);
                gameObjectPiece.Load(gameObjectData);
            }
        }
        loadFinished?.Invoke();
    }

    void Save()
    {
        if (_gameData != null)
        {
            _gameData.gameObjects.Clear();
            foreach (var gameObjectPiece in FindObjectsOfType<GameObjectPiece>(includeInactive: true))
            {
                _gameData.gameObjects.Add(gameObjectPiece.Save());
            }

            var json = JsonUtility.ToJson(_gameData);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            var b64 = System.Convert.ToBase64String(plainTextBytes);
            string androidDemoPath = Application.persistentDataPath + "/LevelData.json";

            using StreamWriter streamWriter = new StreamWriter(androidDemoPath);
            streamWriter.Write(b64);
        }
    }

    bool IsGameDataLoaded()
    {
        string androidDemoPath = Application.persistentDataPath + "/LevelData.json";
        if (File.Exists(androidDemoPath))
        {
            using StreamReader streamReader = new StreamReader(androidDemoPath);
            var b64 = streamReader.ReadToEnd();
            var plainTextBytes = System.Convert.FromBase64String(b64);
            string json = System.Text.Encoding.UTF8.GetString(plainTextBytes);

            _gameData = JsonUtility.FromJson<GameData>(json);
            _gameData.SetExistingData(_gameData);
            CurrentLevel = _gameData.currentLevel;

            return true;
        }
        return false;
    }

    void OnApplicationFocus() => Save();

    void OnDisable() => SceneManager.sceneLoaded -= OnLevelFinishedLoading;
}
