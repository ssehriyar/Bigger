using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/* Use this class to create triangles */
public class TriangleCreater : MonoBehaviour
{
    //void Awake()
    //{
    //    Vector2[] triPos1 = { new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0f) };
    //    Vector2[] triPos2 = { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f) };
    //    Vector2[] triPos3 = { new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f) };
    //    Vector2[] triPos4 = { new Vector2(1f, 0f), new Vector2(0.5f, 0.5f), new Vector2(1f, 1f) };
    //    List<Vector2[]> trianglePositions = new List<Vector2[]>();
    //    trianglePositions.Add(triPos1);
    //    trianglePositions.Add(triPos2);
    //    trianglePositions.Add(triPos3);
    //    trianglePositions.Add(triPos4);
    //    for (int index = 3; index < 4; index++)
    //    {
    //        Mesh mesh = new Mesh();

    //        Vector3[] vertices = new Vector3[3];
    //        Vector2[] uv = new Vector2[3];
    //        int[] triangles = new int[3];

    //        uv = triPos1;
    //        // Set vertices in the array
    //        for (int i = 0; i < 3; i++)
    //        {
    //            vertices[i] = trianglePositions[index][i];
    //            uv[i] = trianglePositions[index][i];
    //        }
    //        triangles[0] = 0;
    //        triangles[1] = 1;
    //        triangles[2] = 2;

    //        mesh.Clear();
    //        mesh.vertices = vertices;
    //        mesh.triangles = triangles;
    //        mesh.RecalculateNormals();
    //        AssetDatabase.CreateAsset(mesh, "Assets/Triangle3Mesh.asset");

    //        //var go = Instantiate(Resources.Load("Triangles/" + index)) as GameObject;
    //        //go.GetComponent<MeshFilter>().mesh = mesh;
    //        //go.GetComponent<PolygonCollider2D>().points = uv;
    //    }
    //}
}
