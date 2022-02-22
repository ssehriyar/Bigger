using System.Collections.Generic;
using UnityEngine;

public enum TrianglePos
{
    Bottom = 0, Left = 1, Top = 2, Right = 3
}

public class Triangle
{
    private TrianglePos _myPos;
    private int _gameObjectId = -1;
    private Vector2Int _position;
    private GameObject _go;

    public Triangle(int x, int y, TrianglePos trianglePos)
    {
        _position = new Vector2Int(x, y);
        _myPos = trianglePos;

    }
    public GameObject Go
    {
        get
        {
            return _go;
        }
        set
        {
            _go = value;
        }
    }

    public Vector2Int Position
    {
        get
        {
            return _position;
        }
    }

    public TrianglePos MyPos
    {
        get
        {
            return _myPos;
        }
    }

    public int ParentGameObjectId
    {
        get
        {
            return _gameObjectId;
        }
        set
        {
            _gameObjectId = value;
        }
    }
}