using System;
using UnityEngine;

public class DotController : MonoBehaviour
{
    BoardEndController boardEndController;

    void OnEnable() => SnapController.checkGameObjects += CheckGameObjects;

    void Start()
    {
        boardEndController = FindObjectOfType<BoardEndController>();
        boardEndController.GameObjects.Add(this, false);
    }

#if UNITY_ANDROID
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
#endif

    void CheckGameObjects()
    {
        bool[] controlList = new bool[4];
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 0.3f, LayerMask.GetMask("GoChild"));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<MeshDrag>() != null)
            {
                controlList[int.Parse(hitCollider.name)] = true;
            }
        }
        var falsePosInArray = Array.IndexOf(controlList, false);
        if (falsePosInArray == -1)
            boardEndController.GameEndControl(this, true);
        else
            boardEndController.GameEndControl(this, false);
    }

    void OnDisable() => SnapController.checkGameObjects -= CheckGameObjects;
}
