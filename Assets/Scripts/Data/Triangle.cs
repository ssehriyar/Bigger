using UnityEngine;

public enum TrianglePos
{
    Bottom = 0, Left = 1, Top = 2, Right = 3
}

public class Triangle
{
    public TrianglePos trianglePosition;
    public int ParentGameObjectId = -1;
    public Vector2Int position;
    public GameObject gameObject;

    public Triangle(int trianglePos)
    {
        this.trianglePosition = (TrianglePos)trianglePos;
    }
}