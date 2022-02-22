using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEndController : MonoBehaviour
{
    [SerializeField]private int counter = 0;

    private void Start()
    {
        SnapController.CheckEndingEvent += TruePosControl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("GoChild"))
        {
            counter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("GoChild"))
        {
            counter--;
        }
    }

    private void OnDestroy()
    {
        SnapController.CheckEndingEvent -= TruePosControl;
    }

    public void TruePosControl()
    {
        if(counter == 4)
        {
            GameManager.Instance.GameObjectPos(gameObject.name, true);
        }
        else
        {
            GameManager.Instance.GameObjectPos(gameObject.name, false);
        }
    }
}
