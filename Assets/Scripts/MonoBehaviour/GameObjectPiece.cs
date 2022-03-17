using UnityEngine;

public class GameObjectPiece : MonoBehaviour
{
    GameObjectData gameObjectData;
    bool loaded = false;

    void Start()
    {
        gameObjectData = new GameObjectData();
        if (!loaded)
        {
            var children = ParentNull();
            SetCollidersAndPosStart();
            SetParent(children);
        }
    }

    Transform[] ParentNull()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int index = 0; index < children.Length; index++)
        {
            children[index] = transform.GetChild(0);
            transform.GetChild(0).SetParent(null, true);
        }
        return children;
    }

    void SetParent(Transform[] children)
    {
        foreach (Transform child in children)
        {
            child.SetParent(transform, true);
        }
    }

    void SetCollidersAndPosStart()
    {
        Collider2D[] allColliders = GetComponents<CircleCollider2D>();
        Vector3 middlePoint = Vector3.zero;

        foreach (Collider2D collider2D in allColliders)
        {
            middlePoint += collider2D.bounds.center;
        }

        middlePoint /= allColliders.Length;
        middlePoint.z = 0;
        foreach (Collider2D collider in allColliders)
        {
            collider.offset -= (Vector2)middlePoint;
        }
        transform.position += middlePoint;
    }

    public GameObjectData Save()
    {
        gameObjectData.Name = name;
        gameObjectData.Position = transform.position;
        return gameObjectData;
    }

    public void Load(GameObjectData gameObjectData)
    {
        loaded = true;
        this.gameObjectData = gameObjectData;
        var children = ParentNull();
        SetCollidersAndPosStart();
        SetParent(children);
        transform.position = gameObjectData.Position;
    }
}
