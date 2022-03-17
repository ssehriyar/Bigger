using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public Transform GameObjectsTransform;
    [SerializeField] MyColors _myColors;
    [SerializeField] Transform dotsTransform;

    public GameObject[] GameObjects { get; private set; }

    void OnEnable() => GamePersist.creatingObjects += CreateScene;

    void CreateScene(int boardSize, int gameObjectNumber)
    {
        int posX, posY, trianglePos;
        float threshold = 0.6f;
        float circleColliderRadius = 0.2f;
        var borderoffset = BorderOffset(boardSize);

        GameObjects = new GameObject[gameObjectNumber];
        Square[,] myBoard = new Square[boardSize, boardSize];
        Color[] colors = new Color[gameObjectNumber];
        _myColors.UniqueRandomColors(colors);

        List<List<Vector2>> createdColliders = new List<List<Vector2>>();
        Queue<Tuple<Triangle, GameObject, int>> que = new Queue<Tuple<Triangle, GameObject, int>>();

        var go = Instantiate(Resources.Load("Prefabs/Background/Board")) as GameObject;
        go.transform.position = new Vector3(boardSize * 0.5f + borderoffset, boardSize * 0.5f, 15f);//SELIM screen size a gore position set
        go.transform.localScale = new Vector3(boardSize + 0.2f, boardSize + 0.2f, 0f);

        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                go = Instantiate(Resources.Load("Prefabs/Background/Dot"), dotsTransform) as GameObject;
                go.name = x + "" + y;
                go.transform.position = new Vector3(x + 0.5f + borderoffset, y + 0.5f, 14f);
                go.transform.localScale = new Vector3(0.2f, 0.2f, 0f);

                myBoard[x, y] = new Square();

                for (int index = 0; index < 4; index++)
                {
                    go = Instantiate(Resources.Load("Prefabs/Triangles/" + index)) as GameObject;
                    go.name = index.ToString();
                    go.transform.position += new Vector3(x + borderoffset, y, -0.1f);
                    myBoard[x, y].GetTriangle(index).gameObject = go;
                    myBoard[x, y].GetTriangle(index).position = new Vector2Int(x, y);
                }
            }
        }

        for (int index = 0; index < gameObjectNumber; index++)
        {
            go = Instantiate(Resources.Load("Prefabs/GameObject/GameObjectPiece")) as GameObject;
            go.transform.SetParent(GameObjectsTransform, true);
            //go.transform.localScale *= 100;
            go.name = "GameObject" + index;
            GameObjects[index] = go;
            do
            {
                posX = RandomUtil.Instance.Range(0, boardSize - 1);
                posY = RandomUtil.Instance.Range(0, boardSize - 1);
                trianglePos = RandomUtil.Instance.Range(0, 4);

            } while (myBoard[posX, posY].GetTriangle(trianglePos).ParentGameObjectId != -1);

            Triangle triangle = myBoard[posX, posY].GetTriangle(trianglePos);
            triangle.ParentGameObjectId = index;
            triangle.gameObject.transform.SetParent(go.transform, false);
            //triangle.gameObject.GetComponent<MeshRenderer>().material.color = colors[index];
            triangle.gameObject.GetComponent<SpriteRenderer>().material.color = colors[index];

            var circleCol = go.GetComponent<CircleCollider2D>();
            circleCol.radius = circleColliderRadius;
            circleCol.offset = new Vector2(triangle.position.x + 0.5f + borderoffset, posY + 0.5f);
            var dotGo = Instantiate(Resources.Load("Prefabs/GameObject/Dot"), go.transform) as GameObject;
            dotGo.transform.localPosition = new Vector3(circleCol.offset.x, circleCol.offset.y, 0.2f);

            List<Vector2> colPos = new List<Vector2> { circleCol.offset };
            createdColliders.Add(colPos);

            foreach (Triangle neighbor in Neighbors(triangle, myBoard))
            {
                if (neighbor.ParentGameObjectId == -1)
                {
                    que.Enqueue(new Tuple<Triangle, GameObject, int>(neighbor, go, index));
                }
            }
        }

        while (que.Count != 0)
        {
            Tuple<Triangle, GameObject, int> curTri = que.Dequeue();
            if (curTri.Item1.ParentGameObjectId != -1)
                continue;

            if (RandomUtil.Instance.Value() < threshold)
            {
                curTri.Item1.ParentGameObjectId = curTri.Item3;
                curTri.Item1.gameObject.transform.SetParent(curTri.Item2.transform, false);
                //curTri.Item1.gameObject.GetComponent<MeshRenderer>().material.color = colors[curTri.Item3];
                curTri.Item1.gameObject.GetComponent<SpriteRenderer>().material.color = colors[curTri.Item3];

                if (!createdColliders[curTri.Item3].Contains(new Vector2(curTri.Item1.position.x + 0.5f + borderoffset, curTri.Item1.position.y + 0.5f)))
                {
                    Vector2 colliderOffsetPos = new Vector2(curTri.Item1.position.x + 0.5f + borderoffset, curTri.Item1.position.y + 0.5f);
                    CircleCollider2D newCollider = curTri.Item2.AddComponent<CircleCollider2D>();
                    newCollider.isTrigger = false;
                    newCollider.radius = circleColliderRadius;
                    newCollider.offset = colliderOffsetPos;
                    createdColliders[curTri.Item3].Add(colliderOffsetPos);
                    var dotGo = Instantiate(Resources.Load("Prefabs/GameObject/Dot"), curTri.Item2.transform) as GameObject;
                    dotGo.transform.localPosition = new Vector3(newCollider.offset.x, newCollider.offset.y, 0.2f);
                }

                foreach (Triangle neighbor in Neighbors(curTri.Item1, myBoard))
                {
                    if (neighbor.ParentGameObjectId == -1)
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

    float BorderOffset(int boardSize)
    {
        return boardSize switch
        {
            5 => -0.5f,
            6 => -1f,
            _ => 0f,
        };
    }

    List<Triangle> Neighbors(Triangle triangle, Square[,] myBoard)
    {
        List<Triangle> myNeighbors = new List<Triangle>();
        switch (triangle.trianglePosition)
        {
            case TrianglePos.Bottom:
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Left));
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Right));

                if (triangle.position.y > 0)
                    myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y - 1].GetTriangle((int)TrianglePos.Top));
                break;

            case TrianglePos.Left:
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Bottom));
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Top));

                if (triangle.position.x > 0)
                    myNeighbors.Add(myBoard[triangle.position.x - 1, triangle.position.y].GetTriangle((int)TrianglePos.Right));
                break;

            case TrianglePos.Top:
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Left));
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Right));

                if (triangle.position.y < myBoard.GetLength(0) - 1)
                    myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y + 1].GetTriangle((int)TrianglePos.Bottom));
                break;

            case TrianglePos.Right:
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Bottom));
                myNeighbors.Add(myBoard[triangle.position.x, triangle.position.y].GetTriangle((int)TrianglePos.Top));

                if (triangle.position.x < myBoard.GetLength(0) - 1)
                    myNeighbors.Add(myBoard[triangle.position.x + 1, triangle.position.y].GetTriangle((int)TrianglePos.Left));
                break;
        }
        return myNeighbors;
    }

    void OnDisable() => GamePersist.creatingObjects -= CreateScene;
}
