using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyColors", menuName = "Color")]
public class MyColors : ScriptableObject
{
    public List<Color> colors = new List<Color>();

    public Color GetRandomColor()
    {
        return colors[RandomUtil.Instance.Range(0, colors.Count)];
    }

    public void UniqueRandomColors(Color[] colorArr)
    {
        int start = RandomUtil.Instance.Range(0, colors.Count);
        for(int index = 0; index < colorArr.Length; index++, start++)
        {
            if(start == colors.Count)
            {
                start = 0;
            }
            colorArr[index] = colors[start];
        }
    }
}