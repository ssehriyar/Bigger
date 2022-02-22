using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{
    private CircleCollider2D[] allCollidersArray;
    private Vector3 offsetPos;
    private Vector3 collidersCenter;
    private int counter = 0;

    public delegate void CheckEnding();
    public static event CheckEnding CheckEndingEvent;

    private void Start()
    {
        allCollidersArray = GetComponents<CircleCollider2D>();
        collidersCenter = new Vector3();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Grid"))
        {
            counter++;
            collidersCenter += collision.bounds.center;
        }
        if(counter == GetComponents<CircleCollider2D>().Length)
        {
            offsetPos = FindCenterPoint(collidersCenter) - GetCenterPoint();
            offsetPos.z = transform.position.z;
            transform.position += offsetPos;

            CheckEndingEvent?.Invoke();
            counter = 0;
            collidersCenter = Vector3.zero;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Grid"))
        {
            counter = 0;
            collidersCenter = Vector3.zero;
        }
    }

    public Vector3 GetCenterPoint()
    {
        Vector3 center = new Vector3();
        for(int i = 0; i < allCollidersArray.Length; i++)
        {
            center += allCollidersArray[i].bounds.center;
        }
        return (center / allCollidersArray.Length);
    }

    public Vector3 FindCenterPoint(Vector3 vec)
    {
        Vector3 center = vec / allCollidersArray.Length;
        return center;
    }
}
