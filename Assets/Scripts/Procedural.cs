using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Procedural : MonoBehaviour
{
    private int boardSize = 4;
    private Square[,] myBoard;
    private const float threshold = 0.6f;
    private float circleColliderRadius = 0.2f;
    [SerializeField] private MyColors myColors;
    [SerializeField] private Material _defaulMat;

    List<Vector2[]> trianglePositions = new List<Vector2[]>();

    private void Awake()
    {
        _ = GameManager.Instance;
    }
    public void ProceduralStart()
    {
        Vector2[] triPos1 = { new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0f) };
        Vector2[] triPos2 = { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f) };
        Vector2[] triPos3 = { new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f) };
        Vector2[] triPos4 = { new Vector2(1f, 0f), new Vector2(0.5f, 0.5f), new Vector2(1f, 1f) };
        trianglePositions.Add(triPos1);
        trianglePositions.Add(triPos2);
        trianglePositions.Add(triPos3);
        trianglePositions.Add(triPos4);
        boardSize = GameManager.Instance.BoardSize;
        CreateBoard();
        CreateGoPiece(GameManager.Instance.GameObjectPieceNumber);
    }

    private void CreateBoard()
    {
        myBoard = new Square[boardSize, boardSize];
        for(int x = 0; x < boardSize; x++)
        {
            for(int y = 0; y < boardSize; y++)
            {
                myBoard[x, y] = new Square(x, y);
            }
        }
        CallMeshCreator();
    }

    private void CallMeshCreator()
    {
        for(int x = 0; x < boardSize; x++)
        {
            for(int y = 0; y < boardSize; y++)
            {
                for(int i = 0; i < 4; i++)
                {
                    MeshCreater(myBoard[x, y].GetTriangle((TrianglePos)i));
                }
            }
        }
    }

    private void MeshCreater(Triangle triangle)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        // Set vertices in the array
        for(int i = 0; i < 3; i++)
        {
            vertices[i] = trianglePositions[(int)triangle.MyPos][i];
            uv[i] = trianglePositions[(int)triangle.MyPos][i];
        }
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        Vector3 moveXY = new Vector3(triangle.Position.x, triangle.Position.y, -1f);

        GameObject go = new GameObject("Go", typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody2D), typeof(SortingGroup), typeof(PolygonCollider2D), typeof(MeshDrag));
        go.transform.localScale = Vector3.one;
        go.transform.position = moveXY;
        go.tag = "GoChild";
        go.layer = LayerMask.NameToLayer("GoChild");
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        go.GetComponent<MeshRenderer>().material = _defaulMat;
        go.GetComponent<PolygonCollider2D>().points = uv;
        go.GetComponent<PolygonCollider2D>().isTrigger = false;
        triangle.Go = go;
    }

    private void CreateGoPiece(int goNumber)
    {
        Color[] randomColors = new Color[goNumber];
        myColors.UniqueRandomColors(randomColors);
        List<List<Vector2>> createdColliders = new List<List<Vector2>>();
        Queue<Tuple<Triangle, GameObject, int>> que = new Queue<Tuple<Triangle, GameObject, int>>();

        for(int i = 0; i < goNumber; i++)
        {
            GameObject go = new GameObject("GO" + i, typeof(Rigidbody2D), typeof(SnapController), typeof(GameObjectPiece), typeof(CircleCollider2D));
            go.transform.position = new Vector3(0f, 0f, 0f);
            go.layer = LayerMask.NameToLayer("GoParent");
            go.tag = "GoParent";
            go.GetComponent<CircleCollider2D>().radius = circleColliderRadius;
            go.GetComponent<CircleCollider2D>().isTrigger = false;
            go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            int x, y;
            TrianglePos pos;

            do
            {
                x = RandomUtil.Instance.Range(0, boardSize - 1);
                y = RandomUtil.Instance.Range(0, boardSize - 1);
                pos = (TrianglePos)RandomUtil.Instance.Range(0, 3);

            } while(myBoard[x, y].GetTriangle(pos).ParentGameObjectId != -1);

            Triangle firstTriangle = myBoard[x, y].GetTriangle(pos);
            firstTriangle.ParentGameObjectId = i;

            go.GetComponent<CircleCollider2D>().offset += new Vector2(x + 0.5f, y + 0.5f);
            List<Vector2> colPos = new List<Vector2>();
            colPos.Add(new Vector2(x + 0.5f, y + 0.5f));
            createdColliders.Add(colPos);

            firstTriangle.Go.transform.SetParent(go.transform);

            firstTriangle.Go.GetComponent<SortingGroup>().sortingOrder = i;
            firstTriangle.Go.GetComponent<SortingGroup>().sortingLayerName = i.ToString();
            firstTriangle.Go.GetComponent<MeshRenderer>().material.color = randomColors[i];

            foreach(Triangle neighbor in Neighbors(firstTriangle))
            {
                if(neighbor.ParentGameObjectId == -1)
                {
                    que.Enqueue(new Tuple<Triangle, GameObject, int>(neighbor, go, i));
                }
            }
        }

        while(que.Count != 0)
        {
            Tuple<Triangle, GameObject, int> curTri = que.Dequeue();
            if(curTri.Item1.ParentGameObjectId != -1)
                continue;

            if(RandomUtil.Instance.Value() < threshold)
            {
                curTri.Item1.Go.transform.SetParent(curTri.Item2.transform);
                curTri.Item1.ParentGameObjectId = curTri.Item3;

                curTri.Item1.Go.GetComponent<SortingGroup>().sortingLayerName = curTri.Item3.ToString();
                curTri.Item1.Go.GetComponent<SortingGroup>().sortingOrder = curTri.Item3;

                curTri.Item1.Go.GetComponent<MeshRenderer>().material.color = randomColors[curTri.Item3];

                if(!createdColliders[curTri.Item3].Contains(new Vector2(curTri.Item1.Position.x + 0.5f, curTri.Item1.Position.y + 0.5f)))
                {
                    Vector2 colliderOffsetPos = new Vector2(curTri.Item1.Position.x + 0.5f, curTri.Item1.Position.y + 0.5f);
                    CircleCollider2D newCollider = curTri.Item2.AddComponent<CircleCollider2D>();
                    newCollider.isTrigger = false;
                    newCollider.radius = circleColliderRadius;
                    newCollider.offset = colliderOffsetPos;
                    createdColliders[curTri.Item3].Add(new Vector2(curTri.Item1.Go.transform.position.x + 0.5f, curTri.Item1.Go.transform.position.y + 0.5f));
                }

                foreach(Triangle neighbor in Neighbors(curTri.Item1))
                {
                    if(neighbor.ParentGameObjectId == -1)
                    {
                        que.Enqueue(new Tuple<Triangle, GameObject, int>(neighbor, curTri.Item2, curTri.Item3));
                    }
                }
            }
            else
            {
                que.Enqueue(curTri);
            }
        }
    }

    private List<Triangle> Neighbors(Triangle triangle)
    {
        List<Triangle> myNeighbors = new List<Triangle>();
        switch(triangle.MyPos)
        {
            case TrianglePos.Bottom:
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Left));
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Right));
                if(triangle.Position.y > 0)
                {
                    myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y - 1].GetTriangle(TrianglePos.Top));
                }
                break;

            case TrianglePos.Left:
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Bottom));
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Top));
                if(triangle.Position.x > 0)
                {
                    myNeighbors.Add(myBoard[triangle.Position.x - 1, triangle.Position.y].GetTriangle(TrianglePos.Right));
                }
                break;

            case TrianglePos.Top:
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Left));
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Right));
                if(triangle.Position.y < boardSize - 1)
                {
                    myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y + 1].GetTriangle(TrianglePos.Bottom));
                }
                break;

            case TrianglePos.Right:
                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Bottom));

                myNeighbors.Add(myBoard[triangle.Position.x, triangle.Position.y].GetTriangle(TrianglePos.Top));
                if(triangle.Position.x < boardSize - 1)
                {
                    myNeighbors.Add(myBoard[triangle.Position.x + 1, triangle.Position.y].GetTriangle(TrianglePos.Left));
                }
                break;
        }
        return myNeighbors;
    }
}