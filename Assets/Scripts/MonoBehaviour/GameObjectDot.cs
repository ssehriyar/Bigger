using UnityEngine;

public class GameObjectDot : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenSelf()
    {
        gameObject.SetActive(true);
    }

    public void CloseSelf()
    {
        gameObject.SetActive(false);
    }
}
