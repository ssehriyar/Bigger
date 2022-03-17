using System;

public class Square
{
	readonly Triangle[] _triangles;

    public Square()
    {
        _triangles = new Triangle[4];

        for(int index = 0; index < _triangles.Length; index++)
        {
            Triangle temp = new Triangle(index);
            _triangles[index] = temp;
        }
    }

    public Triangle GetTriangle(int index)
    {
        if(index > 3 || index < 0)
        {
            throw new ArgumentOutOfRangeException("GetTriangle index was out of bounds -> Exception");
        }
        return _triangles[index];
    }
}