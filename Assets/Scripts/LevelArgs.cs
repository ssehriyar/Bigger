using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelArgs
{
    public int seed;
    public int goNumber;
    public int boardSize;

    public LevelArgs(int seed, int goNumber, int boardSize)
    {
        this.seed = seed;
        this.goNumber = goNumber;
        this.boardSize = boardSize;
    }
}
