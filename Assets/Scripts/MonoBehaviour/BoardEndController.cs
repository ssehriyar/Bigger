using System.Collections.Generic;
using UnityEngine;

public class BoardEndController : MonoBehaviour
{
    [SerializeField] NewLevel newLevel;
    int counter = 0;

    public Dictionary<DotController, bool> GameObjects { get; set; } = new Dictionary<DotController, bool>();

    public void GameEndControl(DotController gridController, bool b)
    {
        counter++;
        GameObjects[gridController] = b;
        if (counter == GameObjects.Count)
        {
            counter = 0;
            if (!GameObjects.ContainsValue(false))
            {
                GameObjects.Clear();
                newLevel.LoadNewLevel();
            }
        }
    }
}
