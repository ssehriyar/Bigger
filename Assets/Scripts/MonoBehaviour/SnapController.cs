using UnityEngine;

public class SnapController : MonoBehaviour
{
    CircleCollider2D[] allCollidersArray;
    Vector3 offsetPos;
    Vector3 collidersCenter;
    int counter = 0;

	public delegate void CheckGameObjects();
	public static CheckGameObjects checkGameObjects;

	void Start()
    {
        allCollidersArray = GetComponents<CircleCollider2D>();
        collidersCenter = new Vector3();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<DotController>() != null)
        {
            counter++;
            collidersCenter += collision.bounds.center;
        }
        // The last collider will enter this if
        if(counter == allCollidersArray.Length)
        {
            // Put GameObject to right position with colliders position
            offsetPos = FindCenterPoint(collidersCenter) - GetCenterPoint();
            offsetPos.z = 0;
            transform.position += offsetPos;

            // Check if the game ends
            checkGameObjects?.Invoke();
            counter = 0;
            collidersCenter = Vector3.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<DotController>() != null)
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
