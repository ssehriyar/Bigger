using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MeshDrag : MonoBehaviour
{
    public delegate void DragEndedDelegate(MeshDrag meshDrag);

    public DragEndedDelegate dragEndedCallback;

    private Vector3 _dragOffset;
    private Camera _cam;
    private CircleCollider2D[] parentColliders;
    private string holdMySortingLayerName;
    private SortingGroup[] allSiblings;

    private void Start()
    {
        parentColliders = GetComponentsInParent<CircleCollider2D>();
        _cam = Camera.main;
        holdMySortingLayerName = GetComponent<SortingGroup>().sortingLayerName;
        allSiblings = transform.parent.GetComponentsInChildren<SortingGroup>();
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnMouseDown()
    {
        foreach(SortingGroup sortingGroup in allSiblings)
        {
            sortingGroup.sortingLayerName = "Selected";
        }
        _dragOffset = transform.parent.position - GetMousePos();
        foreach(CircleCollider2D collider2D in parentColliders)
        {
            collider2D.isTrigger = false;
        }
    }

    private void OnMouseDrag()
    {
        transform.parent.position = GetMousePos() + _dragOffset;
    }


    private void OnMouseUp()
    {
        foreach(SortingGroup sortingGroup in allSiblings)
        {
            sortingGroup.sortingLayerName = holdMySortingLayerName;
        }
        foreach(CircleCollider2D collider2D in parentColliders)
        {
            collider2D.isTrigger = true;
        }
    }

    private Vector3 GetMousePos()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
