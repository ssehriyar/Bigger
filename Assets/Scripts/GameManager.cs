using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum BoardLevel
{
    Easy = 4, Medium = 5, Hard = 6
};

public class GameManager : Singleton<GameManager>
{

    private int _boardSize = 4;
    public int BoardSize
    {
        get
        {
            return _boardSize;
        }
        set
        {
            _boardSize = value;
        }
    }
    private int _goPieceNumber = 5;
    public int GameObjectPieceNumber
    {
        get
        {
            return _goPieceNumber;
        }
        private set
        {
            _goPieceNumber = value;
        }
    }
    private int _currentLevel = 1;
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            _currentLevel = value;
        }
    }
    private int _leftPieceCounter = 0;
    private Dictionary<string, bool> goPosDictionary;

    private GameObject newLevel;
    private string demoPath = "/LevelOutput.txt";

    private void Start()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        Debug.Log("GameManager Start() is called.");
        StartScene(GetLevelArgs());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene _, LoadSceneMode __)
    {
        StartScene(GetLevelArgs());
    }

    private void SetValues(LevelArgs o)
    {
        if(o.boardSize == -1 || o.goNumber == -1)
        {
            switch(BoardSize)
            {
                case (int)BoardLevel.Easy:
                    o.goNumber = RandomUtil.Instance.Range(5, 8);
                    o.boardSize = (int)BoardLevel.Medium;
                    break;
                case (int)BoardLevel.Medium:
                    o.goNumber = RandomUtil.Instance.Range(8, 10);
                    o.boardSize = (int)BoardLevel.Hard;
                    break;
                case (int)BoardLevel.Hard:
                    o.goNumber = RandomUtil.Instance.Range(10, 12);
                    o.boardSize = (int)BoardLevel.Easy;
                    break;
                default:
                    o.goNumber = RandomUtil.Instance.Range(5, 8);
                    o.boardSize = (int)BoardLevel.Easy;
                    break;
            }
        }
    }

    void StartScene(LevelArgs o)
    {
        RandomUtil.Instance.Seed(o.seed);

        SetValues(o);
        BoardSize = o.boardSize;

        GameObjectPieceNumber = o.goNumber;

        /* This part for level output format */
        /* It writes the values in project folder and Resources/demoPath */

        string json = UnityEngine.JsonUtility.ToJson(o);

        /* Use this part one editor mode. */
        string level = Application.dataPath + "/Resources" + demoPath;

        /* Use this part on Android */
        //String level = Application.persistentDataPath + demoPath;

        if(level != null)
        {
            using(StreamWriter writer = new StreamWriter(level))
            {
                writer.WriteLine(json);
                writer.Close();
            }
        }

        _leftPieceCounter = BoardSize * BoardSize;
        Grid grid = new Grid(BoardSize);
        goPosDictionary = new Dictionary<string, bool>();
        GameObject.Find("Procedural").GetComponent<Procedural>().ProceduralStart();
        newLevel = GameObject.Find("FadeInOut");
        Debug.Log("Current Level: " + CurrentLevel++);
    }

    private LevelArgs GetLevelArgs()
    {
        LevelArgs lvlArgs = null;
        /* This part reads the file*/
        //try
        //{
        //    String level = Application.dataPath + "/Resources" + demoPath;
        //    //String level = Application.persistentDataPath + demoPath;
        //    if(level != null)
        //    {
        //        using(StreamReader sr = new StreamReader(level/*new MemoryStream(level.bytes)*/))
        //        {
        //            var readText = sr.ReadToEnd();
        //            lvlArgs = UnityEngine.JsonUtility.FromJson<LevelArgs>(readText);
        //            sr.Close();
        //        }
        //    }
        //}
        //catch(System.Exception) { }
        //finally
        //{
        //    if(lvlArgs == null)
        //    {
                lvlArgs = new LevelArgs(UnityEngine.Random.Range(0, int.MaxValue), -1, -1);
        //    }
        //}
        return lvlArgs;
    }

    public void GameObjectPos(string goName, bool b)
    {
        if(goPosDictionary.ContainsKey(goName))
        {
            goPosDictionary[goName] = b;
        }
        else
        {
            goPosDictionary.Add(goName, b);

        }
        if(goPosDictionary.Count == BoardSize * BoardSize)
        {
            foreach(KeyValuePair<string, bool> pair in goPosDictionary)
            {
                if(pair.Value)
                    _leftPieceCounter--;
            }
            if(_leftPieceCounter == 0)
            {
                _currentLevel++;
                newLevel.GetComponent<NewLevel>().LoadNewLevel();
            }
            else
            {
                _leftPieceCounter = BoardSize * BoardSize;
            }
        }
    }
}
