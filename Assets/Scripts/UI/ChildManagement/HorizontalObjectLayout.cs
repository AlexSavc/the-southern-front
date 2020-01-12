using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObjectLayout : MonoBehaviour
{
    public enum Alignment { Left, Center, Right, Top, Bottom }
    //public Alignment alignment = Alignment.Left;
    public Vector2 offset;
    [Tooltip("Max items on the alignment axis")]
    public int maxOnAxis = 4;
    private List<GameObject> items;

    void Start()
    {
        items = Utility.GetChildren(transform);
    }

    void OnValidate()
    {
        items = Utility.GetChildren(transform);
        Refresh();
    }

    public void Refresh()
    {
        Vector2 origin = GetStartOffset();

        for(int i = 0; i < transform.childCount; i++)
        {

            Vector2 pos = Vector2.zero;//GetPosition(i);

            pos.x = origin.x + pos.x + GetLocalXOffset(i);
            pos.y = origin.y - pos.y;

            transform.GetChild(i).localPosition = pos;
        }
    }

    public void AddObject(Transform toAdd)
    {
        toAdd.transform.SetParent(transform);
        Refresh();
    }

    private Vector2 GetPosition(int index)
    {
        Vector2 pos = new Vector2(0, 0);

        pos.x = ((index % 4)) * offset.x;
        pos.y = (Utility.RoundUpInt(index / 4)) * offset.y;

        return pos;
    }

    private Vector2 GetStartOffset()
    {
        Vector2 pos = new Vector2(0, 0);

        pos.x = -(offset.x * maxOnAxis / 2.5f);
        pos.y = (offset.y * (transform.childCount / 4))/2;

        return pos;
    }

    private float GetLocalXOffset(int index)
    {
        if (items.Count - index <= items.Count % maxOnAxis)
        {
            if (items.Count % maxOnAxis == 1) return maxOnAxis/2.5f * offset.x;
            return (maxOnAxis / (items.Count % maxOnAxis) * offset.x)/2;
        }
        else return 0;
    }
}