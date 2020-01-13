using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObjectLayout : MonoBehaviour
{
    public enum Alignment { Center,  Top, Bottom }
    public Alignment VerticalAlignment = Alignment.Center;
    public Vector2 offset;
    [Tooltip("Max items on the alignment axis")]
    public int maxOnAxis = 4;
    private List<GameObject> items;
    public bool debug = false;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        items = Utility.GetChildren(transform);
        Vector2 origin = GetStartOffset();

        for(int i = 0; i < transform.childCount; i++)
        {

            Vector2 pos = GetPosition(i);

            pos.x = origin.x + pos.x + GetLocalXOffset(i);
            if (VerticalAlignment == Alignment.Bottom) { pos.y = origin.y - pos.y; }
            else /*(VerticalAlignment == Alignment.Top)*/ { pos.y = origin.y + pos.y; }
            
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
        if (VerticalAlignment == Alignment.Center)
        pos.y = (offset.y * (transform.childCount / 4))/2;

        return pos;
    }

    private float GetLocalXOffset(int index)
    {
        if (items.Count < 1) return 0;
        if (items.Count - index <= items.Count % maxOnAxis)
        {
            if (items.Count % maxOnAxis == 1) return maxOnAxis/2.5f * offset.x;
            return (maxOnAxis / (items.Count % maxOnAxis) * offset.x)/2;
        }
        else return 0;
    }
}