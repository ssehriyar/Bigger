using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MeshDrag : MonoBehaviour
{
    Camera _cam;
    Transform myParent;
    Vector3 _dragOffset;
    CreateObjects createObjects;
    ScrollRect scrollRect;

    GameObject[] parentGameObjects;
    GameObjectDot[] gameObjectDots;
    CircleCollider2D[] parentColliders;

    public delegate void CloseTrigger();
    public static CloseTrigger closeTrigger;

    void Start()
    {
        _cam = Camera.main;
        scrollRect = FindObjectOfType<ScrollRect>();
        parentColliders = GetComponentsInParent<CircleCollider2D>();

        myParent = transform.parent;
        gameObjectDots = myParent.GetComponentsInChildren<GameObjectDot>(includeInactive: true);
        createObjects = FindObjectOfType<CreateObjects>();
        parentGameObjects = createObjects.GameObjects;
    }

    void OnMouseDown()
    {
        scrollRect.enabled = false;
        transform.parent.SetParent(null);
        int zPos = 1;
        foreach (GameObject go in parentGameObjects)
        {
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, zPos++);
        }
        myParent.position = new Vector3(myParent.position.x, myParent.position.y, 0);

        foreach (GameObjectDot goDots in gameObjectDots)
        {
            goDots.OpenSelf();
        }
        _dragOffset = transform.parent.position - GetMousePos();

        foreach (CircleCollider2D collider2D in parentColliders)
        {
            collider2D.isTrigger = false;
        }
    }

    void OnMouseDrag()
    {
        transform.parent.position = GetMousePos() + _dragOffset;
    }

    void OnMouseUp()
    {
        foreach (CircleCollider2D collider2D in parentColliders)
        {
            collider2D.isTrigger = true;
        }
        foreach (GameObjectDot goDots in gameObjectDots)
        {
            goDots.CloseSelf();
        }
        scrollRect.enabled = true;
    }

    Vector3 GetMousePos()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
