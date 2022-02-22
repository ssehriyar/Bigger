using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private float circleColRad = 1.5f;

    public Grid(int size)
    {
        GameObject border = new GameObject("Border", typeof(SpriteRenderer));
        border.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/White_1x1");
        border.GetComponent<SpriteRenderer>().color = Color.black;
        border.transform.position = new Vector3(size * 0.5f + BorderOffset(size), size * 0.5f, 11f);
        border.transform.localScale = new Vector3(size + 0.2f, size + 0.2f, 0f);
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                GameObject go = new GameObject("Grid" + x + y, typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(BoardEndController));
                go.layer = LayerMask.NameToLayer("Board");
                go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Circle");
                go.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.black, 0.9f);
                go.GetComponent<CircleCollider2D>().radius = circleColRad;
                go.transform.position = new Vector3(x + 0.5f + BorderOffset(size), y + 0.5f, 10f);
                go.transform.localScale = new Vector3(0.2f, 0.2f, 0f);
                go.tag = "Grid";
            }
        }
    }

    private float BorderOffset(int borderSize)
    {
        switch(borderSize)
        {
            case 4:
                return 0f;
            case 5:
                return -0.5f;
            case 6:
                return -1f;
            default:
                return 0f;
        }
    }
}
