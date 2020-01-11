using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObjectLayout : MonoBehaviour
{
    [SerializeField]
    private float spacing;
    public enum Alignment { Left, Center, Right, Top, Bottom }
    public Alignment alignment;
    public Vector2 offset;

    public void Refresh()
    {
        for(int i = 0; i < transform.childCount; i++)
        {

        }
    }

    public void AddObject(Transform toAdd)
    {

    }
}
