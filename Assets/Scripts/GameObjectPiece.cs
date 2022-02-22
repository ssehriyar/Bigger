using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPiece : MonoBehaviour
{
    private Camera _cam;
    private Vector3 screenBounds;
    private Collider2D[] allColliders;
    private void Start()
    {
        _cam = Camera.main;
        screenBounds = _cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)) - _cam.transform.position;
        float randX = RandomUtil.Instance.Range(0.2f, screenBounds.x * 0.5f);
        float randY = RandomUtil.Instance.Range(-screenBounds.y * 0.5f, -3);

        allColliders = GetComponents<CircleCollider2D>();
        Vector3 middlePoint = Vector3.zero;
        foreach(Collider2D collider2D in allColliders)
        {
            middlePoint += collider2D.bounds.center;
        }
        middlePoint /= allColliders.Length;
        middlePoint = new Vector3(randX, randY, transform.position.z) - middlePoint;
        transform.position += middlePoint;
    }
}
