using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Square
{
    private Triangle[] _triangles;

    public Square(int x, int y)
    {
        _triangles = new Triangle[4];

        for(int index = 0; index < _triangles.Length; index++)
        {
            Triangle temp = new Triangle(x, y, (TrianglePos)index);
            _triangles[index] = temp;

        }
    }

    public Triangle GetTriangle(TrianglePos index)
    {
        if((int)index > 3 || (int)index < 0)
        {
            throw new ArgumentOutOfRangeException("GetTriangle index was out of bounds -> Exception");

        }
        return _triangles[(int)index];
    }
}









